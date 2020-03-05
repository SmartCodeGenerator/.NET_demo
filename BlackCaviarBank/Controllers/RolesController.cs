using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize(Roles = "admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesManagementService rolesManagementService;

        public RolesController(IRolesManagementService rolesManagementService)
        {
            this.rolesManagementService = rolesManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles() 
        {
            return Ok(await rolesManagementService.GetAppRoles());
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string name)
        {
            await rolesManagementService.CreateRole(name);
            return RedirectToAction(nameof(GetAllRoles));
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            await rolesManagementService.DeleteRole(roleId);
            return NoContent();
        }
    }
}