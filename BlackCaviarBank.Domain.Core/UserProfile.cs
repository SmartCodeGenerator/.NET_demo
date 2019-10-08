using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlackCaviarBank.Domain.Core
{
    public class UserProfile : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<SubscriptionSubscriber> SubscriptionSubscribers { get; set; } = new List<SubscriptionSubscriber>();
    }
}
