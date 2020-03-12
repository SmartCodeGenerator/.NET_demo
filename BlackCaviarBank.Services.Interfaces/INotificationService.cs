using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetNotifications();
        Task<Notification> GetNotificationById(Guid id);
        Task<IEnumerable<Notification>> GetNotificationsForUser(UserProfile user);
        Task<Notification> NotifyUser(UserProfile user, UserNotificationDTO userNotification);
        Task NotifySubscribers(SubscribersNotificationDTO subscribersNotification);
        void DeleteNotification(Guid id);
    }
}
