using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class RolesManagementService : IRolesManagementService
    {
        public async Task<IEnumerable<IdentityError>> CreateRole(string roleName, RoleManager<IdentityRole> manager)
        {
            var result = await manager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
            {
                return null;
            }
            return result.Errors;
        }

        public async Task<IEnumerable<IdentityError>> DeleteRole(string roleId, RoleManager<IdentityRole> manager)
        {
            var role = await manager.FindByIdAsync(roleId);

            var result = await manager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return null;
            }
            return result.Errors;
        }
    }
}
