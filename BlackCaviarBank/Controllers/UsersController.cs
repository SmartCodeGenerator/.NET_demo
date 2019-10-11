using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly SignInManager<UserProfile> signInManager;
        private readonly UnitOfWork unitOfWork;

        public UsersController(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
        }

        [HttpGet("Login/{id}")]
        public ActionResult<UserProfile> Login(string id) => Ok(unitOfWork.UserProfiles.Get(id));

        [HttpPost("Login")]
        //[ValidateAntiForgeryToken]
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
                var result = await signInManager.PasswordSignInAsync(userData.UserName, userData.Password, userData.RememberMe.Value, false);
                if (result.Succeeded)
                {
                    var currentUser = await userManager.GetUserAsync(HttpContext.User);
                    return Content($"{currentUser.UserName} has been signed in");
                }
                else
                {
                    ModelState.AddModelError("wrong userdata", "Wrong username and(or) password");
                    return Conflict(ModelState);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPost("LogOut")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                await signInManager.SignOutAsync();
                return Content($"{currentUser.UserName} has been signed out");
            }
            else
            {
                return NotFound("No user is logged in");
            }
        }

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
                var config = new MapperConfiguration(c => c.CreateMap<RegisterUserDTO, UserProfile>()).CreateMapper();
                var user = config.Map<RegisterUserDTO, UserProfile>(userData);

                IdentityResult result = await userManager.CreateAsync(user, userData.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);

                    await unitOfWork.Save();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }

                return CreatedAtAction(nameof(Login), new { id = user.Id }, user);
            }
            else
            {
                return Conflict(ModelState);
            }
        }
    }
}