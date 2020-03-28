using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly ISubscriptionService subscriptionService;

        public ServicesController(UserManager<UserProfile> userManager, UnitOfWork unitOfWork, ISubscriptionService subscriptionService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.subscriptionService = subscriptionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            return Ok(await subscriptionService.GetServices());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(Guid id)
        {
            return Ok(await subscriptionService.GetService(id));
        }

        [Authorize]
        [HttpGet("UserSubscriptions")]
        public async Task<IActionResult> GetUserSubscriptions()
        {
            return Ok(await subscriptionService.GetUserSubscriptions(await userManager.GetUserAsync(User)));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("RegisterService")]
        public async Task<IActionResult> CreateService(ServiceDTO data)
        {
            if (ModelState.IsValid)
            {
                var result = await subscriptionService.CreateService(data);
                await unitOfWork.SaveChanges();

                if (result != null)
                {
                    return CreatedAtAction(nameof(GetService), new { id = result.ServiceId }, result);
                }
                else
                {
                    return BadRequest(data);
                }
            }
            return Conflict(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(ServiceDTO data, Guid id)
        {
            if (ModelState.IsValid)
            {
                await subscriptionService.UpdateService(data, id);
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveService(Guid id)
        {
            subscriptionService.RemoveService(id);
            await unitOfWork.SaveChanges();

            return NoContent();
        }

        [Authorize]
        [HttpPut("SubscribeOnService")]
        public async Task<IActionResult> SubcribeOnService(SubscriptionDTO data)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                await subscriptionService.SubscribeOnService(user, (await unitOfWork.Cards.Get(c => c.CardNumber.Equals(data.CardNumber))).First(), data.ServiceId);
                unitOfWork.UserProfiles.Update(user);
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [Authorize]
        [HttpPut("UnsubscribeFromService/{id}")]
        public async Task<IActionResult> UnsubscribeFromService(Guid id)
        {
            var user = await userManager.GetUserAsync(User);
            await subscriptionService.UnsubscribeFromService(user, id);
            unitOfWork.UserProfiles.Update(user);
            await unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}