using System.ComponentModel.DataAnnotations;

namespace BlackCaviarBank.Services.Interfaces.Resources.DTOs
{
    public class TransactionDTO
    {
        [Required, StringLength(20, MinimumLength = 16)]
        public string From { get; set; }

        [Required, StringLength(20, MinimumLength = 16)]
        public string To { get; set; }

        [Required, Range(0, double.MaxValue)]
        public double Amount { get; set; }
    }
}
