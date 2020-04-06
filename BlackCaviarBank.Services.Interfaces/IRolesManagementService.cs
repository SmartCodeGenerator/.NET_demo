using BlackCaviarBank.Domain.Core.QueryParams;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IRolesManagementService
    {
        Task<PagedList<IdentityRole>> GetAppRoles(RoleParams roleParams);
        Task CreateRole(string roleName);
        Task DeleteRole(string roleId);
    }
}
