﻿using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class BankingNotificationService : INotificationService
    {
        private readonly IRepository<Notification> notificationRepository;
        private readonly IRepository<Service> serviceRepository;

        public BankingNotificationService(IRepository<Notification> notificationRepository, IRepository<Service> serviceRepository)
        {
            this.notificationRepository = notificationRepository;
            this.serviceRepository = serviceRepository;
        }

        public void DeleteNotification(Guid id)
        {
            notificationRepository.Delete(id);
        }

        public async Task<Notification> GetNotificationById(Guid id)
        {
            return await notificationRepository.GetById(id);
        }

        public IEnumerable<Notification> GetNotificationsForUser(UserProfile user)
        {
            return notificationRepository.Get(n => n.ReceiverId == user.Id);
        }

        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            return await notificationRepository.GetAll();
        }

        public async Task NotifySubscribers(SubscribersNotificationDTO subscribersNotification)
        {
            var notification = new Notification();

            var service = await serviceRepository.GetById(subscribersNotification.ServiceSenderId);
            notification.Text = subscribersNotification.Text;
            
            notification.Sender = service;
            
            foreach(var pair in service.SubscriptionSubscribers)
            {
                notification.Time = DateTime.UtcNow;
                notification.ReceiverId = pair.SubscriberId;

                await notificationRepository.Create(notification);
            }
        }

        public async Task<Notification> NotifyUser(UserProfile user, UserNotificationDTO userNotification)
        {
            var notification = new Notification();

            var service = await serviceRepository.GetById(userNotification.ServiceSenderId);
            notification.Text = userNotification.Text;
            notification.Time = DateTime.UtcNow;

            notification.Sender = service;
            notification.Receiver = user;

            await notificationRepository.Create(notification);

            return notification;
        }
    }
}
