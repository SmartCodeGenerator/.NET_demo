﻿using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using System.Collections.Generic;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class NotificationRepository : IRepository<Notification, int>
    {
        private readonly ApplicationContext context;

        public NotificationRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<Notification> GetAll() => context.Notifications;

        public Notification Get(int id) => context.Notifications.Find(id);

        public void Create(Notification notification) => context.Notifications.Add(notification);

        public void Update(Notification notification) => context.Notifications.Update(notification);

        public void Delete(int id)
        {
            var notificationToRemove = Get(id);
            if (notificationToRemove != null)
            {
                context.Notifications.Remove(notificationToRemove);
            }
        }
    }
}
