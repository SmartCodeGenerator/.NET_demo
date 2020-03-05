using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IRolesManagementService
    {
        Task<IEnumerable<IdentityRole>> GetAppRoles();
        Task CreateRole(string roleName);
        Task DeleteRole(string roleId);
    }
}
