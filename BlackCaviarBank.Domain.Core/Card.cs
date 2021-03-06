﻿using System;

namespace BlackCaviarBank.Domain.Core
{
    public class Card
    {
        public Guid CardId { get; set; }
        public string CardNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string PaymentSystem { get; set; }
        public string CVV2 { get; set; }
        public double Balance { get; set; }
        public bool IsBlocked { get; set; }

        public string OwnerId { get; set; }
        public UserProfile Owner { get; set; }
    }
}
