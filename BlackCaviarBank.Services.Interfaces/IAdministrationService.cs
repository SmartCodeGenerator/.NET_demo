using BlackCaviarBank.Domain.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAdministrationService
    {
        Task<IEnumerable<UserProfile>> GetUserProfiles();
        Task<UserProfile> GetUserProfileInfo(string userId);
        Task AssignRolesToUser(string userId, IList<string> roles);
        Task BanUserProfile(string userId);
        Task UnbanUserProfile(string userId);
        Task DeleteUserProfile(string userId);
    }
}
