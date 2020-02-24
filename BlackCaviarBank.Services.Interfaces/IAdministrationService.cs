using BlackCaviarBank.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAdministrationService
    {
        public Task<bool> AssignRolesToUser(UserManager<UserProfile> userManager, string userId, IList<string> roles);
        public Task<bool> BanUserProfile(UserManager<UserProfile> userManager, string userId);
        public Task<bool> UnbanUserProfile(UserManager<UserProfile> userManager, string userId);
        public Task<bool> DeleteUserProfile(UserManager<UserProfile> userManager, string userId);
    }
}
