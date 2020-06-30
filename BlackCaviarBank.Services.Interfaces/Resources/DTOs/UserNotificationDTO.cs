using System;
using System.ComponentModel.DataAnnotations;

namespace BlackCaviarBank.Services.Interfaces.Resources.DTOs
{
    public class UserNotificationDTO
    {
        [Required, StringLength(255)]
        public string Text { get; set; }

        [Required]
        public Guid ServiceSenderId { get; set; }
    }
}
