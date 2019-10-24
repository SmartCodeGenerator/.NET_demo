using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
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
    public class ServicesController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ServicesController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Service>> GetAllServices() => unitOfWork.Services.GetAll().ToList();

        [HttpGet("{id}")]
        public ActionResult<Service> GetService(int id) => unitOfWork.Services.Get(id);

        /// <summary>
        /// Creates a specific service.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /RegisterService
        ///     {
        ///        "name": "JotaroKujoDreams",
        ///        "price": 100000
        ///     }
        ///
        /// </remarks>
        [HttpPost("RegisterService")]
        public async Task<ActionResult<Service>> CreateService(ServiceDTO data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                ModelState.AddModelError("name", "Service name must not be empty");
            }

            if (ModelState.IsValid)
            {
                var service = mapper.Map<Service>(data);

                unitOfWork.Services.Create(service);

                await unitOfWork.Save();

                return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        /// <summary>
        /// Updates a specific service.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /UpdateService
        ///     {
        ///        "name": "JotaroKujoDreams",
        ///        "price": 100000
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<Service>> UpdateService(ServiceDTO data, int id)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                ModelState.AddModelError("name", "Service name must not be empty");
            }

            if (ModelState.IsValid)
            {
                var service = unitOfWork.Services.Get(id);

                service.Name = data.Name == "string" ? service.Name : data.Name;
                service.Price = data.Price == 0 ? service.Price : data.Price;

                unitOfWork.Services.Update(service);

                await unitOfWork.Save();

                return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveService(int id)
        {
            unitOfWork.Services.Delete(id);

            await unitOfWork.Save();

            return Ok($"Service with id ${id} has been removed");
        }
    }
}