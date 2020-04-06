using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> GetAllRoles([FromQuery] RoleParams roleParams) 
        {
            var result = await rolesManagementService.GetAppRoles(roleParams);

            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(result);
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