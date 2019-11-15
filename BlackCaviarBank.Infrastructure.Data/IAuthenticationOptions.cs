using System.Security.Claims;

namespace BlackCaviarBank.Infrastructure.Data
{
    public interface IAuthenticationOptions
    {
        string SecretKey { get; set; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }
        Claim[] Claims { get; set; }
    }
}
