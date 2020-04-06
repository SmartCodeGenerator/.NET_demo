using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class AdministrationService : IAdministrationService
    {
        private readonly IRepository<UserProfile> repository;
        private readonly UserManager<UserProfile> userManager;

        public AdministrationService(IRepository<UserProfile> repository, UserManager<UserProfile> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public async Task AssignRolesToUser(string userId, IList<string> roles)
        {
            UserProfile user = await userManager.FindByIdAsync(userId);

            IList<string> userRoles = await userManager.GetRolesAsync(user);

            IEnumerable<string> addedRoles = roles.Except(userRoles);

            IEnumerable<string> removedRoles = userRoles.Except(roles);

            await userManager.AddToRolesAsync(user, addedRoles);

            await userManager.RemoveFromRolesAsync(user, removedRoles);
        }

        public async Task BanUserProfile(string userId)
        {
            UserProfile user = await userManager.FindByIdAsync(userId);
            IList<string> userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains("admin"))
            {
                user.IsBanned = true;

                await userManager.UpdateAsync(user);
            }
        }

        public async Task DeleteUserProfile(string userId)
        {
            UserProfile user = await userManager.FindByIdAsync(userId);
            IList<string> userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains("admin"))
            {
                await userManager.DeleteAsync(user);
            }
        }

        public async Task<UserProfile> GetUserProfileInfo(string userId)
        {
            return await repository.GetById(Guid.Parse(userId));
        }

        public async Task<PagedList<UserProfile>> GetUserProfiles(UserProfileParams userProfileParams)
        {
            return await repository.GetAll(userProfileParams);
        }

        public async Task UnbanUserProfile(string userId)
        {
            UserProfile user = await userManager.FindByIdAsync(userId);

            if (user.IsBanned.Value)
            {
                user.IsBanned = false;

                await userManager.UpdateAsync(user);
            }
        }
    }
}
