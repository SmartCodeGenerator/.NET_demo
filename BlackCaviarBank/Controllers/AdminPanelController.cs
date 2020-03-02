using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize(Roles = "admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPanelController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly IAdministrationService administrationService;

        public AdminPanelController(UserManager<UserProfile> userManager, IAdministrationService administrationService)
        {
            this.userManager = userManager;
            this.administrationService = administrationService;
        }

        [HttpGet("UserProfiles")]
        public async Task<IActionResult> GetUserProfiles()
        {
            return Ok(await administrationService.GetUserProfiles());
        }

        [HttpGet("UserProfiles/{id}")]
        public async Task<IActionResult> GetUserProfileInfo(string userId)
        {
            return Ok(await administrationService.GetUserProfileInfo(userId));
        }

        [HttpPut("AssignRolesToUser/{id}")]
        public async Task<IActionResult> AssignRolesToUser(string userId, List<string> roles)
        {
            if (await administrationService.AssignRolesToUser(userManager, userId, roles))
            {
                return NoContent();
            }
            return NotFound(userId);
        }

        [HttpPut("BanUser/{id}")]
        public async Task<IActionResult> BanUser(string userId)
        {
            if (await administrationService.BanUserProfile(userManager, userId))
            {
                return NoContent();
            }
            return BadRequest(userId);
        }

        [HttpPut("UnbanUser/{id}")]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            if (await administrationService.UnbanUserProfile(userManager, userId))
            {
                return NoContent();
            }
            return BadRequest(userId);
        }

        [HttpDelete("UserProfiles/{id}")]
        public async Task<IActionResult> DeleteUserProfile(string userId)
        {
            if (await administrationService.DeleteUserProfile(userManager, userId))
            {
                return NoContent();
            }
            return BadRequest(userId);
        }
    }
}