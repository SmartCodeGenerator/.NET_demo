using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class BankingNotificationService : INotificationService
    {
        public List<Notification> NotifyAll(Service sender, string message)
        {
            var data = new List<Notification>();
            var notification = new Notification { Text = message, Time = DateTime.Now, Sender = sender };

            foreach(var link in sender.SubscriptionSubscribers)
            {
                notification.Receiver = link.Subscriber;
                link.Subscriber.Notifications.Add(notification);
                sender.Notifications.Add(notification);
                data.Add(notification);
            }

            return data;
        }

        public Notification Notify(Service sender, UserProfile receiver, string message)
        {
            var notification = new Notification { Text = message, Time = DateTime.Now, Sender = sender, Receiver = receiver };
            receiver.Notifications.Add(notification);
            sender.Notifications.Add(notification);

            return notification;
        }
    }
}
