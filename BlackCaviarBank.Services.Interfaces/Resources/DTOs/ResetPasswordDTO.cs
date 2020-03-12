using System.ComponentModel.DataAnnotations;

namespace BlackCaviarBank.Services.Interfaces.Resources.DTOs
{
    public class ResetPasswordDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords doesn't match each other")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
