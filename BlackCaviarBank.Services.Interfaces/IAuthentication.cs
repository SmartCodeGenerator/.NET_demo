using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data;
using System.Collections.Generic;
using System.Security.Claims;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAuthentication
    {
        string SecretKey { get; set; }
        bool IsTokenValid(string token);
        string GenerateToken(IAuthenticationOptions options);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
