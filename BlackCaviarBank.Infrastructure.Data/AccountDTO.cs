namespace BlackCaviarBank.Infrastructure.Data
{
    public class AccountDTO
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public double? Balance { get; set; }
        public double? InterestRate { get; set; }
    }
}
