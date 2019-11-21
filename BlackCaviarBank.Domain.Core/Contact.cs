namespace BlackCaviarBank.Domain.Core
{
    public class Contact
    {
        public string OwnerId { get; set; }
        public UserProfile Owner { get; set; }

        public string ContactId { get; set; }
        public UserProfile ContactProfile { get; set; }
    }
}
