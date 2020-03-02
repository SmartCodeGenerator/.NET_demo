using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize(Roles = "admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRolesManagementService rolesManagementService;

        public RolesController(RoleManager<IdentityRole> roleManager, IRolesManagementService rolesManagementService)
        {
            this.roleManager = roleManager;
            this.rolesManagementService = rolesManagementService;
        }

        [HttpGet]
        public IActionResult GetAllRoles() 
        {
            return Ok(roleManager.Roles.ToList());
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("roleName", "NotNull");
            }

            var errors = await rolesManagementService.CreateRole(name, roleManager);

            if (errors == null)
            {
                return RedirectToAction(nameof(GetAllRoles));
            }
            else
            {
                foreach(var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return Conflict(ModelState);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var errors = await rolesManagementService.DeleteRole(roleId, roleManager);

            if (errors == null)
            {
                return NoContent();
            }
            else
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return Conflict(ModelState);
        }
    }
}