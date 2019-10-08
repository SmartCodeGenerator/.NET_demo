using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public ActionResult<List<UserProfile>> GetUserProfiles() => unitOfWork.UserProfiles.GetAll().ToList();

        [HttpPost]
        public async Task<ActionResult<UserProfile>> Register()
        {
            string userName = Request.Form.FirstOrDefault(u => u.Key.ToLower().Equals("username")).Value;
            string email = Request.Form.FirstOrDefault(u => u.Key.ToLower().Equals("email")).Value;
            string password = Request.Form.FirstOrDefault(u => u.Key.ToLower().Equals("password")).Value;
            string passwordConfirm = Request.Form.FirstOrDefault(u => u.Key.ToLower().Equals("passwordconfirm")).Value;
            string firstName = Request.Form.FirstOrDefault(u => u.Key.ToLower().Equals("firstname")).Value;
            string lastName = Request.Form.FirstOrDefault(u => u.Key.ToLower().Equals("lastname")).Value;

            if (string.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "UserName must not be empty");
            }
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "Email must not be empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "Password must not be empty");
            }
            if (string.IsNullOrEmpty(passwordConfirm))
            {
                ModelState.AddModelError("passwordConfirm", "Password confirmation must not be empty");
            }
            if (string.IsNullOrEmpty(firstName))
            {
                ModelState.AddModelError("firstName", "FirstName must not be empty");
            }
            if (string.IsNullOrEmpty(lastName))
            {
                ModelState.AddModelError("lastName", "LastName must not be empty");
            }
            if (!password.Equals(passwordConfirm))
            {
                ModelState.AddModelError("confirmation", "Password must match the confirmation");
            }

            if (ModelState.IsValid)
            {
                var user = new UserProfile() { UserName = userName, Email = email, FirstName = firstName, LastName = lastName };

                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);

                    unitOfWork.UserProfiles.Create(user);
                    await unitOfWork.Save();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }

                return CreatedAtAction(nameof(GetUserProfiles), user);
            }
            else
            {
                return Conflict(ModelState);
            }
        }
    }
}