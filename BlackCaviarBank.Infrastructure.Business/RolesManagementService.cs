using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class RolesManagementService : IRolesManagementService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesManagementService(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task CreateRole(string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        public async Task DeleteRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                await roleManager.DeleteAsync(role);
            }
        }

        public async Task<PagedList<IdentityRole>> GetAppRoles(RoleParams roleParams)
        {
            var roles = await roleManager.Roles.ToListAsync();
            return new PagedList<IdentityRole>(roles, roles.Count, roleParams.PageNumber, roleParams.PageSize);
        }
    }
}
