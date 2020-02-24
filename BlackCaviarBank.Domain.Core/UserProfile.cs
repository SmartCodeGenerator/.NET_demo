using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlackCaviarBank.Domain.Core
{
    public class UserProfile : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsBanned { get; set; }
        public byte[] ProfileImage { get; set; }

        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<SubscriptionSubscriber> SubscriptionSubscribers { get; set; } = new List<SubscriptionSubscriber>();
    }
}
