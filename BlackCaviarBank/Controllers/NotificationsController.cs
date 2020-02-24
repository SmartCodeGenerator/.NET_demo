using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork.Implementations;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly INotificationService notifier;

        public NotificationsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, INotificationService notifier)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.notifier = notifier;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetAllNotifications()
        {
            return Ok(unitOfWork.Notifications.GetAll().ToList());
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public IActionResult GetNotification(Guid id)
        {
            return Ok(unitOfWork.Notifications.GetById(id));
        }

        [Authorize]
        [HttpGet("ForCurrentUser")]
        public async Task<IActionResult> GetAllForCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);

            return Ok(unitOfWork.Notifications.Get(n => n.ReceiverId.Equals(user.Id)));
        }

        [HttpPost("SendForUser")]
        public async Task<ActionResult<Notification>> SendNotificationForUser(UserNotificationDTO data)
        {
            if (ModelState.IsValid)
            {
                var notification = notifier.Notify(unitOfWork.Services.GetById(data.ServiceSenderId), unitOfWork.UserProfiles.GetById(Guid.Parse(data.UserReceiverId)), data.Text);
                
                unitOfWork.Notifications.Create(notification);
                await unitOfWork.SaveChanges();
                
                return CreatedAtAction(nameof(GetNotification), new { id = notification.NotificationId }, notification);
            }
            return Conflict(ModelState);
        }

        [HttpPost("SendForSubscribers")]
        public async Task<ActionResult<List<Notification>>> SendNotificationForSubscribers(SubscribersNotificationDTO data)
        {
            if (User.IsInRole("admin"))
            {
                if (string.IsNullOrEmpty(data.Text))
                {
                    ModelState.AddModelError("text", "notification text must not be empty");
                }

                if (ModelState.IsValid)
                {
                    var service = unitOfWork.Services.Get(data.ServiceSenderId);

                    if (service != null)
                    {
                        var notifications = notifier.NotifyAll(service, data.Text);
                        foreach (var notification in notifications)
                        {
                            unitOfWork.Notifications.Create(notification);
                        }
                        await unitOfWork.Save();
                        return Ok(notifications);
                    }
                    else
                    {
                        return NotFound($"there is no service with id {data.ServiceSenderId}");
                    }
                }
                else
                {
                    return Conflict(ModelState);
                }
            }
            else
            {
                throw new UnauthorizedAccessException("You must be admin");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotification(int id)
        {
            if (User.IsInRole("admin"))
            {
                var notification = unitOfWork.Notifications.Get(id);

                if (notification != null)
                {
                    var time = notification.Time;
                    unitOfWork.Notifications.Delete(id);
                    await unitOfWork.Save();
                    return Ok($"notification with id {id} from {time} has been deleted");
                }
                else
                {
                    return NotFound($"there is no notification with id {id}");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("You must be admin");
            }
        }
    }
}