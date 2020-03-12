using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRegistrationService registrationService;
        private readonly IAuthenticationService authenticationService;

        public UsersController(IRegistrationService registrationService, IAuthenticationService authenticationService)
        {
            this.registrationService = registrationService;
            this.authenticationService = authenticationService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginUserDTO userData)
        {
            if (ModelState.IsValid)
            {
                string accessToken = await authenticationService.Authenticate(userData);
                if (string.IsNullOrEmpty(accessToken))
                {
                    return BadRequest("Invalid username or password");
                }
                return Ok(accessToken);
            }
            return Conflict(ModelState);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO userData)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<IdentityError> errors = await registrationService.Register(userData);
                if (errors == null)
                {
                    return RedirectToAction(nameof(SignIn), new { userData = new LoginUserDTO { UserName = userData.UserName, Password = userData.Password } });
                }
                foreach(var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return Conflict(ModelState);
        }
    }
}