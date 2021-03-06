﻿using System;

namespace BlackCaviarBank.Domain.Core
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public DateTime? OpeningDate { get; set; }
        public double Balance { get; set; }
        public double InterestRate { get; set; }
        public bool IsBlocked { get; set; }

        public string OwnerId { get; set; }
        public UserProfile Owner { get; set; }
    }
}
