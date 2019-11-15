namespace BlackCaviarBank.Infrastructure.Data
{
    public class UserNotificationDTO
    {
        public string Text { get; set; }
        public int ServiceSenderId { get; set; }
        public string UserReceiverId { get; set; }
    }
}
