using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IRolesManagementService
    {
        IQueryable<IdentityRole> GetAppRoles();
        Task CreateRole(string roleName);
        Task DeleteRole(string roleId);
    }
}
