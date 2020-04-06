using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAdministrationService
    {
        Task<PagedList<UserProfile>> GetUserProfiles(UserProfileParams userProfileParams);
        Task<UserProfile> GetUserProfileInfo(string userId);
        Task AssignRolesToUser(string userId, IList<string> roles);
        Task BanUserProfile(string userId);
        Task UnbanUserProfile(string userId);
        Task DeleteUserProfile(string userId);
    }
}
