using System;

namespace BlackCaviarBank.Domain.Core
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Amount { get; set; }
        public DateTime? Date { get; set; }

        public string PayerId { get; set; }
        public UserProfile Payer { get; set; }
    }
}
