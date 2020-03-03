using System;

namespace BlackCaviarBank.Domain.Core
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public string Text { get; set; }
        public DateTime? Time { get; set; }

        public string ReceiverId { get; set; }
        public UserProfile Receiver { get; set; }

        public Guid SenderId { get; set; }
        public Service Sender { get; set; }
    }
}
