using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork.Implementations;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly UnitOfWork unitOfWork;
        private readonly IAdministrationService administrationService;

        public AdminPanelController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IAdministrationService administrationService)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.administrationService = administrationService;
        }

        [HttpGet("UserProfiles")]
        public IActionResult GetUserProfiles()
        {
            return Ok(unitOfWork.UserProfiles.GetAll());
        }

        [HttpGet("UserProfiles/{id}")]
        public IActionResult GetUserProfileInfo(string userId)
        {
            return Ok(unitOfWork.UserProfiles.GetById(Guid.Parse(userId)));
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