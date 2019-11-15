using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class JWTAuthenticationOptions : IAuthenticationOptions
    {
        public string SecretKey { get; set; } = "H8QRelcPy9NtyzYpqLuqYebStnTO0hs=";
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 10080;
        public Claim[] Claims { get; set; }
    }
}
