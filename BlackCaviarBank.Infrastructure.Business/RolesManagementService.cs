﻿using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
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

        public IQueryable<IdentityRole> GetAppRoles()
        {
            return roleManager.Roles;
        }
    }
}
