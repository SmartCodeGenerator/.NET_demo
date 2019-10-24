using System;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class CardDTO
    {
        public DateTime ExpirationDate { get; set; }
        public string PaymentSystem { get; set; }
        public string CVV2 { get; set; }
        public double Balance { get; set; }
    }
}
