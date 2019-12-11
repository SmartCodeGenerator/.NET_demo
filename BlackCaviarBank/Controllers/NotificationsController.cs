using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
using BlackCaviarBank.Services.Interfaces;
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
        private readonly INotifier notifier;

        public NotificationsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, INotifier notifier)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.notifier = notifier;
        }

        [HttpGet]
        public ActionResult<List<Notification>> GetAllNotifications()
        {
            if (User.IsInRole("admin"))
            {
                return Ok(unitOfWork.Notifications.GetAll().ToList());
            }
            else
            {
                throw new UnauthorizedAccessException("You must be admin");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Notification> GetNotification(int id)
        {
            if (User.IsInRole("admin"))
            {
                var result = unitOfWork.Notifications.Get(id);

                if (result != null)
                {
                    return Ok(result);
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

        [Authorize]
        [HttpGet("ForCurrentUser")]
        public async Task<ActionResult<List<Notification>>> GetAllForCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                return Ok(unitOfWork.Notifications.GetAllForUser(user));
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        [HttpPost("SendForUser")]
        public async Task<ActionResult<Notification>> SendNotificationForUser(UserNotificationDTO data)
        {
            if (User.IsInRole("admin"))
            {
                if (string.IsNullOrEmpty(data.Text))
                {
                    ModelState.AddModelError("text", "notification text must not be empty");
                }

                if (ModelState.IsValid)
                {
                    var user = unitOfWork.UserProfiles.Get(data.UserReceiverId);

                    if (user != null)
                    {
                        var service = unitOfWork.Services.Get(data.ServiceSenderId);

                        if (service != null)
                        {
                            var notification = notifier.Notify(service, user, data.Text);
                            unitOfWork.Notifications.Create(notification);
                            await unitOfWork.Save();
                            return CreatedAtAction(nameof(GetNotification), new { id = notification.NotificationId }, notification);
                        }
                        else
                        {
                            return NotFound($"there is no service with id {data.ServiceSenderId}");
                        }
                    }
                    else
                    {
                        return NotFound($"there is no user with id {data.UserReceiverId}");
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