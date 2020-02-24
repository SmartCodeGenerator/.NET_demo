using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IRolesManagementService
    {
        Task<IEnumerable<IdentityError>> CreateRole(string roleName, RoleManager<IdentityRole> manager);
        Task<IEnumerable<IdentityError>> DeleteRole(string roleId, RoleManager<IdentityRole> manager);
    }
}
