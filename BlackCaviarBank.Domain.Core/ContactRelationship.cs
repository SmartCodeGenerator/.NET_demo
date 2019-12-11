namespace BlackCaviarBank.Domain.Core
{
    public class ContactRelationship
    {
        public int ContactRelationshipId { get; set; }
        public bool IsApproved { get; set; }

        public string SenderId { get; set; }
        public UserProfile User1 { get; set; }

        public string ReceiverId { get; set; }
        public UserProfile User2 { get; set; }
    }
}
