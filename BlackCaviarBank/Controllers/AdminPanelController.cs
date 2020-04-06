using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IAdministrationService administrationService;

        public AdminPanelController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        [HttpGet("UserProfiles")]
        public async Task<IActionResult> GetUserProfiles([FromQuery] UserProfileParams userProfileParams)
        {
            var result = await administrationService.GetUserProfiles(userProfileParams);

            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(result);
        }

        [HttpGet("UserProfiles/{id}")]
        public async Task<IActionResult> GetUserProfileInfo(string userId)
        {
            return Ok(await administrationService.GetUserProfileInfo(userId));
        }

        [HttpPut("AssignRolesToUser/{id}")]
        public async Task<IActionResult> AssignRolesToUser(string userId, List<string> roles)
        {
            await administrationService.AssignRolesToUser(userId, roles);
            return NoContent();
        }

        [HttpPut("BanUser/{id}")]
        public async Task<IActionResult> BanUser(string userId)
        {
            await administrationService.BanUserProfile(userId);
            return NoContent();
        }

        [HttpPut("UnbanUser/{id}")]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            await administrationService.UnbanUserProfile(userId);
            return NoContent();
        }

        [HttpDelete("UserProfiles/{id}")]
        public async Task<IActionResult> DeleteUserProfile(string userId)
        {
            await administrationService.DeleteUserProfile(userId);
            return NoContent();
        }
    }
}