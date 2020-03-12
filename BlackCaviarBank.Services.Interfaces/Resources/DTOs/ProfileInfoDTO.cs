using System.ComponentModel.DataAnnotations;

namespace BlackCaviarBank.Services.Interfaces.Resources.DTOs
{
    public class ProfileInfoDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string FirstName { get; set; }

        [Required, StringLength(20)]
        public string LastName { get; set; }
    }
}
