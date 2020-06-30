using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Business;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlackCaviarBank.Tests
{
    public class BankingNotificationServiceTests
    {
        [Fact]
        public void DeleteNotification_NotificationFound()
        {
            var notifications = new List<Notification>()
            {
                new Notification{ NotificationId = Guid.Parse("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8") }
            };
            var notificationToDelete = notifications.Last();
            var repo = new Mock<IRepository<Notification>>();
            repo.Setup(r => r.Delete(It.IsAny<Guid>())).Callback(() => notifications.Remove(notificationToDelete));
            var service = new BankingNotificationService(repo.Object, null);

            service.DeleteNotification(new Guid("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8"));

            Assert.Empty(notifications);
        }

        [Fact]
        public void DeleteNotification_NotificationNotFound()
        {
            var notifications = new List<Notification>()
            {
                new Notification{ NotificationId = Guid.Parse("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8") }
            };
            var notificationToDelete = notifications.FirstOrDefault(n => n.NotificationId.Equals(new Guid("C1803AB2-9594-4B75-88FE-0FD3375E7228")));
            var repo = new Mock<IRepository<Notification>>();
            repo.Setup(r => r.Delete(It.IsAny<Guid>())).Callback(() => notifications.Remove(notificationToDelete));
            var service = new BankingNotificationService(repo.Object, null);

            service.DeleteNotification(new Guid("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8"));

            Assert.NotEmpty(notifications);
        }

        [Fact]
        public async void GetNotificationById_NotificationFound()
        {
            var notifications = new List<Notification>()
            {
                new Notification{ NotificationId = Guid.Parse("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8") }
            };
            var repo = new Mock<IRepository<Notification>>();
            repo.Setup(r => r.GetById(Guid.Parse("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8"))).ReturnsAsync(notifications.FirstOrDefault(n => n.NotificationId.Equals(Guid.Parse("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8"))));
            var service = new BankingNotificationService(repo.Object, null);

            var result = await service.GetNotificationById(new Guid("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8"));

            Assert.NotNull(result);
        }

        [Fact]
        public async void GetNotificationById_NotificationNotFound()
        {
            var notifications = new List<Notification>()
            {
                new Notification{ NotificationId = Guid.Parse("0DF740D2-421E-4AA7-B1B1-EA71EC0996A8") }
            };
            var repo = new Mock<IRepository<Notification>>();
            repo.Setup(r => r.GetById(Guid.Parse("04F935B3-408F-424C-8784-40A0CD9F487B"))).ReturnsAsync(notifications.FirstOrDefault(n => n.NotificationId.Equals(Guid.Parse("04F935B3-408F-424C-8784-40A0CD9F487B"))));
            var service = new BankingNotificationService(repo.Object, null);

            var result = await service.GetNotificationById(new Guid("04F935B3-408F-424C-8784-40A0CD9F487B"));

            Assert.Null(result);
        }

        private static List<Notification> GetNotifications()
        {
            return new List<Notification>()
            {
                new Notification { ReceiverId = "8034A1D4-EB97-436E-83E2-A23492D34C71" },
                new Notification { ReceiverId = "" },
                new Notification { ReceiverId = "8034A1D4-EB97-436E-83E2-A23492D34C71" },
                new Notification { ReceiverId = "8034A1D4-EB97-436E-83E2-A23492D34C71" },
                new Notification { ReceiverId = "" },
                new Notification { ReceiverId = "8034A1D4-EB97-436E-83E2-A23492D34C71" },
                new Notification { ReceiverId = "" },
                new Notification { ReceiverId = "" },
                new Notification { ReceiverId = "" },
                new Notification { ReceiverId = "8034A1D4-EB97-436E-83E2-A23492D34C71" },
            };
        }

        [Fact]
        public async void GetNotificationsForUser_PagedListCorrectParams()
        {
            var notificationsForCertainUser = GetNotifications().Where(n => n.ReceiverId.Equals("8034A1D4-EB97-436E-83E2-A23492D34C71")).ToList();
            var repo = new Mock<IRepository<Notification>>();
            repo.Setup(r => r.Get(It.IsAny<Func<Notification, bool>>(), It.IsAny<NotificationParams>())).ReturnsAsync(new PagedList<Notification>(notificationsForCertainUser, notificationsForCertainUser.Count, 3, 2));
            var service = new BankingNotificationService(repo.Object, null);

            var result = await service.GetNotificationsForUser(new UserProfile { Id = "8034A1D4-EB97-436E-83E2-A23492D34C71" }, new NotificationParams());

            Assert.NotEmpty(result);
            Assert.Equal(5, result.Count);
            Assert.True(result.HasPrevious);
            Assert.False(result.HasNext);
            Assert.Equal(3, result.TotalPages);
        }

        [Fact]
        public async void GetNotificationsForUser_PagedListEmpty()
        {
            var notificationsForCertainUser = GetNotifications().Where(n => n.ReceiverId.Equals("D1B045D5-E18D-4194-A900-90FB19E93B5B")).ToList();
            var repo = new Mock<IRepository<Notification>>();
            repo.Setup(r => r.Get(It.IsAny<Func<Notification, bool>>(), It.IsAny<NotificationParams>())).ReturnsAsync(new PagedList<Notification>(notificationsForCertainUser, notificationsForCertainUser.Count, 13, 42));
            var service = new BankingNotificationService(repo.Object, null);

            var result = await service.GetNotificationsForUser(new UserProfile { Id = "8034A1D4-EB97-436E-83E2-A23492D34C71" }, new NotificationParams());

            Assert.Empty(result);
            Assert.True(result.HasPrevious);
            Assert.False(result.HasNext);
            Assert.Equal(0, result.TotalPages);
        }

        [Fact]
        public async void GetNotifications_PagedListCorrectParams()
        {
            var repo = new Mock<IRepository<Notification>>();
            repo.Setup(r => r.Get(It.IsAny<Func<Notification, bool>>(), It.IsAny<NotificationParams>())).ReturnsAsync(new PagedList<Notification>(GetNotifications().Skip(8).Take(4).ToList(), GetNotifications().Count, 3, 4));
            var service = new BankingNotificationService(repo.Object, null);

            var result = await service.GetNotificationsForUser(new UserProfile { Id = "8034A1D4-EB97-436E-83E2-A23492D34C71" }, new NotificationParams());

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.True(result.HasPrevious);
            Assert.False(result.HasNext);
            Assert.Equal(3, result.TotalPages);
        }

        [Fact]
        public async void GetNotifications_PagedListEmpty()
        {
            var repo = new Mock<IRepository<Notification>>();
            repo.Setup(r => r.Get(It.IsAny<Func<Notification, bool>>(), It.IsAny<NotificationParams>())).ReturnsAsync(new PagedList<Notification>(GetNotifications().Skip(504).Take(42).ToList(), GetNotifications().Count, 13, 42));
            var service = new BankingNotificationService(repo.Object, null);

            var result = await service.GetNotificationsForUser(new UserProfile { Id = "8034A1D4-EB97-436E-83E2-A23492D34C71" }, new NotificationParams());

            Assert.Empty(result);
            Assert.True(result.HasPrevious);
            Assert.False(result.HasNext);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async void NotifySubscribers_ServiceFound()
        {
            var service = new Service { ServiceId = Guid.Parse("F8C07CB9-FD39-4C15-806B-CBF60D2AD231") };
            var sub1 = new UserProfile { UserName = "Jeffrey" };
            var sub2 = new UserProfile { UserName = "Tony" };
            var ss = new List<SubscriptionSubscriber>()
            {
                new SubscriptionSubscriber { Subscription = service, Subscriber = sub1 },
                new SubscriptionSubscriber { Subscription = service, Subscriber = sub2 }
            };
            service.SubscriptionSubscribers = ss;
            sub1.SubscriptionSubscribers.Add(new SubscriptionSubscriber { Subscription = service, Subscriber = sub1 });
            sub2.SubscriptionSubscribers.Add(new SubscriptionSubscriber { Subscription = service, Subscriber = sub2 });
            var notifications = new List<Notification>();
            var serviceRepo = new Mock<IRepository<Service>>();
            serviceRepo.Setup(sr => sr.GetById(It.IsAny<Guid>())).ReturnsAsync(service);
            var notificationRepo = new Mock<IRepository<Notification>>();
            notificationRepo.Setup(nr => nr.Create(It.IsAny<Notification>())).Callback(() => notifications.Add(new Notification { Text = "test" }));
            var notificationService = new BankingNotificationService(notificationRepo.Object, serviceRepo.Object);

            await notificationService.NotifySubscribers(new SubscribersNotificationDTO { ServiceSenderId = service.ServiceId, Text = "test" });
            var notificationToCheck = notifications.ElementAt(0);

            Assert.Equal(2, notifications.Count);
            Assert.NotNull(notificationToCheck);
            Assert.Equal(notificationToCheck.Text, service.Notifications.Last().Text);
            Assert.Equal(notificationToCheck.Text, sub1.Notifications.Last().Text);
            Assert.Equal(notificationToCheck.Text, sub2.Notifications.Last().Text);
        }

        [Fact]
        public async void NotifySubscribers_ServiceNotFound()
        {
            var serviceRepo = new Mock<IRepository<Service>>();
            serviceRepo.Setup(sr => sr.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);
            var notificationService = new BankingNotificationService(null, serviceRepo.Object);

            await Assert.ThrowsAsync(new NullReferenceException().GetType(), () => notificationService.NotifySubscribers(new SubscribersNotificationDTO { ServiceSenderId = Guid.Parse("F8C07CB9-FD39-4C15-806B-CBF60D2AD231"), Text = "test" }));
        }

        [Fact]
        public async void NotifyUser_ServiceFound()
        {
            var sub = new UserProfile();
            var service = new Service { ServiceId = Guid.Parse("F8C07CB9-FD39-4C15-806B-CBF60D2AD231") };
            bool isCreated = false;
            var serviceRepo = new Mock<IRepository<Service>>();
            serviceRepo.Setup(sr => sr.GetById(It.IsAny<Guid>())).ReturnsAsync(service);
            var notificationRepo = new Mock<IRepository<Notification>>();
            notificationRepo.Setup(nr => nr.Create(It.IsAny<Notification>())).Callback(() => isCreated = true);
            var notificationService = new BankingNotificationService(notificationRepo.Object, serviceRepo.Object);

            var result = await notificationService.NotifyUser(sub, new UserNotificationDTO { ServiceSenderId = service.ServiceId, Text = "test" });

            Assert.Contains(result, sub.Notifications);
            Assert.Contains(result, service.Notifications);
            Assert.True(isCreated);
        }

        [Fact]
        public async void NotifyUser_ServiceNotFound()
        {
            var serviceRepo = new Mock<IRepository<Service>>();
            serviceRepo.Setup(sr => sr.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);
            var notificationService = new BankingNotificationService(null, serviceRepo.Object);

            await Assert.ThrowsAsync(new NullReferenceException().GetType(), () => notificationService.NotifyUser(new UserProfile(), new UserNotificationDTO { ServiceSenderId = Guid.Parse("F8C07CB9-FD39-4C15-806B-CBF60D2AD231"), Text = "test" }));
        }
    }
}
