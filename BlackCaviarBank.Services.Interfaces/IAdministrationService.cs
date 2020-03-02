using BlackCaviarBank.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAdministrationService
    {
        Task<IEnumerable<UserProfile>> GetUserProfiles();
        Task<UserProfile> GetUserProfileInfo(string userId);
        Task<bool> AssignRolesToUser(UserManager<UserProfile> userManager, string userId, IList<string> roles);
        Task<bool> BanUserProfile(UserManager<UserProfile> userManager, string userId);
        Task<bool> UnbanUserProfile(UserManager<UserProfile> userManager, string userId);
        Task<bool> DeleteUserProfile(UserManager<UserProfile> userManager, string userId);
    }
}
