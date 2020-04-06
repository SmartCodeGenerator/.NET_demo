using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly INotificationService notificationService;

        public NotificationsController(UserManager<UserProfile> userManager, UnitOfWork unitOfWork, INotificationService notificationService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.notificationService = notificationService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllNotifications([FromQuery] NotificationParams notificationParams)
        {
            var result = await notificationService.GetNotifications(notificationParams);

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

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(Guid id)
        {
            return Ok(await notificationService.GetNotificationById(id));
        }

        [Authorize]
        [HttpGet("ForCurrentUser")]
        public async Task<IActionResult> GetAllForCurrentUser([FromQuery] NotificationParams notificationParams)
        {
            var result = await notificationService.GetNotificationsForUser(await userManager.GetUserAsync(User), notificationParams);

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

        [Authorize(Roles = "admin")]
        [HttpPost("SendForUser")]
        public async Task<IActionResult> SendNotificationForUser(UserNotificationDTO data)
        {
            if (ModelState.IsValid)
            {
                var result = await notificationService.NotifyUser(await userManager.GetUserAsync(User), data);
                await unitOfWork.SaveChanges();

                if (result != null)
                {
                    return CreatedAtAction(nameof(GetNotification), new { id = result.NotificationId }, result);
                }
                else
                {
                    return BadRequest(data);
                }
            }
            return Conflict(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("SendForSubscribers")]
        public async Task<IActionResult> SendNotificationForSubscribers(SubscribersNotificationDTO data)
        {
            if (ModelState.IsValid)
            {
                await notificationService.NotifySubscribers(data);
                await unitOfWork.SaveChanges();

                return RedirectToAction(nameof(GetAllNotifications));
            }
            return Conflict(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            notificationService.DeleteNotification(id);
            await unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}