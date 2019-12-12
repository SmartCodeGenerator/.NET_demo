using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace BlackCaviarBank.Domain.Core
{
    public class UserProfile : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsBanned { get; set; }
        public byte[] ProfileImage { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<SubscriptionSubscriber> SubscriptionSubscribers { get; set; } = new List<SubscriptionSubscriber>();

        public List<ContactRelationship> Contacts1 { get; set; } = new List<ContactRelationship>();
        public List<ContactRelationship> Contacts2 { get; set; } = new List<ContactRelationship>();
    }
}
