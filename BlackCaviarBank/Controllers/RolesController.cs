using BlackCaviarBank.Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<UserProfile> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<UserProfile> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult<List<IdentityRole>> GetAllRoles() => roleManager.Roles.ToList();

        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("roleName", "Role name must not be empty");
            }

            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllRoles");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Conflict(ModelState);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpDelete("{roleId}")]
        public async Task<ActionResult> DeleteRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                string name = role.Name;

                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return Ok($"Role with name {name} has been deleted");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Conflict(ModelState);
                }
            }
            else
            {
                return NotFound($"There is no role with id {roleId}");
            }
        }
    }
}