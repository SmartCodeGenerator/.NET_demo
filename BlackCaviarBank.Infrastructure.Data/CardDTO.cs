using System;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class CardDTO
    {
        public string CardNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string PaymentSystem { get; set; }
        public string CVV2 { get; set; }
        public double? Balance { get; set; }
    }
}
