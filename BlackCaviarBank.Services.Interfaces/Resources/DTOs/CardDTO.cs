using System.ComponentModel.DataAnnotations;

namespace BlackCaviarBank.Services.Interfaces.Resources.DTOs
{
    public class CardDTO
    {
        [Required]
        [Range(0, double.MaxValue)]
        public double Balance { get; set; }
    }
}
