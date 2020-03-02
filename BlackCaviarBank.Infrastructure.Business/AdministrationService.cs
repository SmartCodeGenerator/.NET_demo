using BlackCaviarBank.Domain.Core;
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

        public AdministrationService(IRepository<UserProfile> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> AssignRolesToUser(UserManager<UserProfile> userManager, string userId, IList<string> roles)
        {
            UserProfile user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                IList<string> userRoles = await userManager.GetRolesAsync(user);

                IEnumerable<string> addedRoles = roles.Except(userRoles);

                IEnumerable<string> removedRoles = userRoles.Except(roles);

                await userManager.AddToRolesAsync(user, addedRoles);

                await userManager.RemoveFromRolesAsync(user, removedRoles);

                return true;
            }
            return false;
        }

        public async Task<bool> BanUserProfile(UserManager<UserProfile> userManager, string userId)
        {
            UserProfile user = await userManager.FindByIdAsync(userId);
            IList<string> userRoles = await userManager.GetRolesAsync(user);

            if (user != null && !userRoles.Contains("admin"))
            {
                user.IsBanned = true;

                await userManager.UpdateAsync(user);

                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUserProfile(UserManager<UserProfile> userManager, string userId)
        {
            UserProfile user = await userManager.FindByIdAsync(userId);
            IList<string> userRoles = await userManager.GetRolesAsync(user);

            if (user != null && !userRoles.Contains("admin"))
            {
                await userManager.DeleteAsync(user);

                return true;
            }
            return false;
        }

        public async Task<UserProfile> GetUserProfileInfo(string userId)
        {
            return await repository.GetById(Guid.Parse(userId));
        }

        public async Task<IEnumerable<UserProfile>> GetUserProfiles()
        {
            return await repository.GetAll();
        }

        public async Task<bool> UnbanUserProfile(UserManager<UserProfile> userManager, string userId)
        {
            UserProfile user = await userManager.FindByIdAsync(userId);

            if (user != null && user.IsBanned.Value)
            {
                user.IsBanned = false;

                await userManager.UpdateAsync(user);

                return true;
            }
            return false;
        }
    }
}
