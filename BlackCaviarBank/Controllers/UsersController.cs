using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Business;
using BlackCaviarBank.Infrastructure.Data;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly SignInManager<UserProfile> signInManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UsersController(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet("Check/{id}")]
        public ActionResult<UserProfile> Check(string id)
        {
            var result = unitOfWork.UserProfiles.Get(id);

            if(result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"there is no user profile with id {id}");
            }
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Login
        ///     {
        ///        "userName": "JoJo",
        ///        "password": "yareyaredaze1",
        ///        "rememberMe": true
        ///     }
        ///
        /// </remarks>
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginUserDTO userData)
        {
            if (string.IsNullOrEmpty(userData.UserName))
            {
                ModelState.AddModelError("username", "UserName must not be empty");
            }
            if (string.IsNullOrEmpty(userData.Password))
            {
                ModelState.AddModelError("password", "Password must not be empty");
            }
            if (!userData.RememberMe.HasValue)
            {
                ModelState.AddModelError("rememberMe", "RememberMe must not be empty");
            }

            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    IAuthenticationOptions options = new JWTAuthenticationOptions
                    {
                        Claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userData.UserName),
                        new Claim("password", userData.Password),
                        new Claim("rememberMe", Convert.ToString(userData.RememberMe))
                    }
                    };
                    IAuthentication authentication = new JWTService(options.SecretKey);
                    string token = authentication.GenerateToken(options);
                    if (!authentication.IsTokenValid(token))
                    {
                        throw new UnauthorizedAccessException();
                    }
                    else
                    {
                        List<Claim> claims = authentication.GetTokenClaims(token).ToList();

                        var userName = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name)).Value;
                        var password = claims.FirstOrDefault(c => c.Type.Equals("password")).Value;
                        var isPersistent = claims.FirstOrDefault(c => c.Type.Equals("rememberMe")).Value;

                        var user = await userManager.FindByNameAsync(userName);
                        if (!user.IsBanned.Value)
                        {
                            var result = await signInManager.PasswordSignInAsync(userName, password, Convert.ToBoolean(isPersistent), false);
                            if (result.Succeeded)
                            {
                                return Ok($"{userName} has been authenticated successfully");
                            }
                            else
                            {
                                ModelState.AddModelError("wrong userdata", "Wrong username and(or) password");

                                return Conflict(ModelState);
                            }
                        }
                        else
                        {
                            return BadRequest($"user {userName} is banned");
                        }
                    }
                }
                else
                {
                    return BadRequest($"{User.Identity.Name} is already authenticated");
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await userManager.GetUserAsync(User);
                await signInManager.SignOutAsync();
                return Content($"{currentUser.UserName} has been signed out");
            }
            else
            {
                return BadRequest("There is no authenticated user to be logged out");
            }
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///        "userName": "JoJo",
        ///        "email": "dio@gmail.com",
        ///        "password": "yareyaredaze1",
        ///        "passwordConfirm": "yareyaredaze1",
        ///        "firstName": "Jotaro",
        ///        "lastName": "Kujo"
        ///     }
        ///
        /// </remarks>
        [HttpPost("Register")]
        public async Task<ActionResult<UserProfile>> Register(RegisterUserDTO userData)
        {
            if (string.IsNullOrEmpty(userData.UserName))
            {
                ModelState.AddModelError("username", "UserName must not be empty");
            }
            if (string.IsNullOrEmpty(userData.Email))
            {
                ModelState.AddModelError("email", "Email must not be empty");
            }
            if (string.IsNullOrEmpty(userData.Password))
            {
                ModelState.AddModelError("password", "Password must not be empty");
            }
            if (string.IsNullOrEmpty(userData.PasswordConfirm))
            {
                ModelState.AddModelError("passwordConfirm", "Password confirmation must not be empty");
            }
            if (string.IsNullOrEmpty(userData.FirstName))
            {
                ModelState.AddModelError("firstName", "FirstName must not be empty");
            }
            if (string.IsNullOrEmpty(userData.LastName))
            {
                ModelState.AddModelError("lastName", "LastName must not be empty");
            }
            if (!userData.Password.Equals(userData.PasswordConfirm))
            {
                ModelState.AddModelError("confirmation", "Password must match the confirmation");
            }

            if (ModelState.IsValid)
            {
                var user = mapper.Map<UserProfile>(userData);

                IdentityResult result = await userManager.CreateAsync(user, userData.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");

                    user.ProfileImage = System.IO.File.ReadAllBytes(@"/MyStaticFiles/images/user_icon.svg");

                    unitOfWork.UserProfiles.Update(user);
                    await unitOfWork.Save();

                    await signInManager.SignInAsync(user, false);

                    return CreatedAtAction(nameof(Check), new { id = user.Id }, user);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return Conflict(ModelState);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }
    }
}