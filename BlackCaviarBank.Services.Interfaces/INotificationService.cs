using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface INotificationService
    {
        Task<PagedList<Notification>> GetNotifications(NotificationParams notificationParams);
        Task<Notification> GetNotificationById(Guid id);
        Task<PagedList<Notification>> GetNotificationsForUser(UserProfile user, NotificationParams notificationParams);
        Task<Notification> NotifyUser(UserProfile user, UserNotificationDTO userNotification);
        Task NotifySubscribers(SubscribersNotificationDTO subscribersNotification);
        void DeleteNotification(Guid id);
    }
}
