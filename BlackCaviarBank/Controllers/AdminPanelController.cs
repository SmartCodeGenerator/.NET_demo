using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize(Roles="admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPanelController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;

        public AdminPanelController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserProfile>>> GetUsers()
        {
            var users = await userManager.GetUsersInRoleAsync("user");

            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("AssignRolesToUser")]
        public async Task<ActionResult> AssignRolesToUser(string userId, List<string> roles)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var rolesToAdd = roles.Except(userRoles);
                var rolesToRemove = userRoles.Except(roles);

                await userManager.AddToRolesAsync(user, rolesToAdd);
                await userManager.RemoveFromRolesAsync(user, rolesToRemove);

                var result = await userManager.GetRolesAsync(user);
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("BanUser")]
        public async Task<ActionResult> BanUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("user"))
                {
                    user.IsBanned = true;

                    unitOfWork.UserProfiles.Update(user);

                    await unitOfWork.Save();

                    return Ok($"User {user.UserName} has been banned");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("UnbanUser")]
        public async Task<ActionResult> UnbanUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("user"))
                {
                    user.IsBanned = false;

                    unitOfWork.UserProfiles.Update(user);

                    await unitOfWork.Save();

                    return Ok($"User {user.UserName} has been unbanned");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}