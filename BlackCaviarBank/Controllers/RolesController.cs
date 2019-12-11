using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        public ActionResult<List<IdentityRole>> GetAllRoles() 
        {
            if (User.IsInRole("admin"))
            {
                return Ok(roleManager.Roles.ToList());
            }
            else
            {
                throw new UnauthorizedAccessException("You must be admin");
            }
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(string name)
        {
            if (User.IsInRole("admin"))
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
                        foreach (var error in result.Errors)
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
            else
            {
                throw new UnauthorizedAccessException("You must be admin");
            }
        }

        [HttpDelete("{roleId}")]
        public async Task<ActionResult> DeleteRole(string roleId)
        {
            if (User.IsInRole("admin"))
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
            else
            {
                throw new UnauthorizedAccessException("You must be admin");
            }
        }
    }
}