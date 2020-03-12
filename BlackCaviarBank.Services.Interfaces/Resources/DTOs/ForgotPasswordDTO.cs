using System.ComponentModel.DataAnnotations;

namespace BlackCaviarBank.Services.Interfaces.Resources.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
