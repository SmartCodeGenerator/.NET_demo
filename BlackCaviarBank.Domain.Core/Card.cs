using System;

namespace BlackCaviarBank.Domain.Core
{
    public class Card
    {
        public int CardId { get; set; }
        public int CardNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string PaymentSystem { get; set; }
        public int CVV2 { get; set; }
        public double Balance { get; set; }

        public string OwnerId { get; set; }
        public UserProfile Owner { get; set; }
    }
}
