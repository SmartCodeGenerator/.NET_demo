using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalCabinetController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IPersonalCabinetService personalCabinetService;
        
        public PersonalCabinetController(UserManager<UserProfile> userManager, UnitOfWork unitOfWork, IPersonalCabinetService personalCabinetService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.personalCabinetService = personalCabinetService;
        }

        [HttpPut("ChangeProfileInfo")]
        public async Task<IActionResult> ChangeProfileInfo(ProfileInfoDTO data)
        {
            if (ModelState.IsValid)
            {
                personalCabinetService.ChangeProfileInfo(await userManager.GetUserAsync(User), data);
                await unitOfWork.SaveChanges();
                return NoContent();
            }
            return Conflict(ModelState);
        }

        [HttpPut("ChangeProfileImage")]
        public async Task<IActionResult> ChangeProfileImage(string path)
        {
            await personalCabinetService.ChangeProfileImage(await userManager.GetUserAsync(User), path);
            await unitOfWork.SaveChanges();
            return NoContent();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO data)
        {
            if (ModelState.IsValid)
            {
                await personalCabinetService.ChangePassword(await userManager.GetUserAsync(User), data);
                await unitOfWork.SaveChanges();
                return NoContent();
            }
            return Conflict(ModelState);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO data)
        {
            if (ModelState.IsValid)
            {
                await personalCabinetService.ForgotPassword(data, "link to form with ResetPasswordDTO");
                await unitOfWork.SaveChanges();
                return NoContent();
            }
            return Conflict(ModelState);
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO data)
        {
            if (ModelState.IsValid)
            {
                await personalCabinetService.ResetPassword(data);
                await unitOfWork.SaveChanges();
                return NoContent();
            }
            return Conflict(ModelState);
        }

        [HttpPut("BlockCard")]
        public async Task<IActionResult> BlockCard(Guid id)
        {
            await personalCabinetService.BlockCard(id);
            await unitOfWork.SaveChanges();
            return NoContent();
        }

        [HttpPut("UnblockCard")]
        public async Task<IActionResult> UnblockCard(Guid id)
        {
            await personalCabinetService.UnblockCard(id);
            await unitOfWork.SaveChanges();
            return NoContent();
        }

        [HttpPut("BlockAccount")]
        public async Task<IActionResult> BlockAccount(Guid id)
        {
            await personalCabinetService.BlockAccount(id);
            await unitOfWork.SaveChanges();
            return NoContent();
        }

        [HttpPut("UnblockAccount")]
        public async Task<IActionResult> UnblockAccount(Guid id)
        {
            await personalCabinetService.UnblockAccount(id);
            await unitOfWork.SaveChanges();
            return NoContent();
        }
    }
}