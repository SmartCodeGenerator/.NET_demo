using System.ComponentModel.DataAnnotations;

namespace BlackCaviarBank.Services.Interfaces.Resources.DTOs
{
    public class AccountDTO
    {
        [Required, StringLength(30)]
        public string Name { get; set; }

        [Required, Range(0, double.MaxValue)]
        public double Balance { get; set; }

        [Required, Range(0, 1)]
        public double InterestRate { get; set; }
    }
}
