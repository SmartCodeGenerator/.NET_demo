using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<string> Authenticate(UserManager<UserProfile> userManager, LoginUserDTO userDTO);
    }
}
