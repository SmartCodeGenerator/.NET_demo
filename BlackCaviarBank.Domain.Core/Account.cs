using System;

namespace BlackCaviarBank.Domain.Core
{
    public class Account
    {
        public int AccountId { get; set; }
        public int AccountNumber { get; set; }
        public string Name { get; set; }
        public DateTime? OpeningDate { get; set; }
        public double Balance { get; set; }
        public double InterestRate { get; set; }

        public string OwnerId { get; set; }
        public UserProfile Owner { get; set; }
    }
}
