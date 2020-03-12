using System;
using System.ComponentModel.DataAnnotations;

namespace BlackCaviarBank.Services.Interfaces.Resources.DTOs
{
    public class SubscriptionDTO
    {
        [Required, StringLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public Guid ServiceId { get; set; }
    }
}
