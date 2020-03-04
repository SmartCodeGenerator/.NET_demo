using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork.Implementations;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ITransactionService operationService;
        private readonly ISubscriptionService serviceHandlingService;

        public ServicesController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IMapper mapper, ITransactionService operationService, ISubscriptionService serviceHandlingService)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
            this.operationService = operationService;
            this.serviceHandlingService = serviceHandlingService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllServices()
        {
            return Ok(unitOfWork.Services.GetAll().ToList());
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetService(Guid id)
        {
            return Ok(unitOfWork.Services.GetById(id));
        }

        [Authorize]
        [HttpGet("UserSubscriptions")]
        public async Task<IActionResult> GetUserSubscriptions()
        {
            var user = await userManager.GetUserAsync(User);

            return Ok(serviceHandlingService.GetSubscriptions(user.Id, unitOfWork.Services.GetAll()));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("RegisterService")]
        public async Task<IActionResult> CreateService(ServiceDTO data)
        {
            if (ModelState.IsValid)
            {
                var service = mapper.Map<Service>(data);

                unitOfWork.Services.Create(service);
                await unitOfWork.SaveChanges();

                return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
            }
            return Conflict(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(ServiceDTO data, Guid id)
        {
            if (ModelState.IsValid)
            {
                var service = unitOfWork.Services.GetById(id);

                mapper.Map(data, service);

                unitOfWork.Services.Update(service);
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveService(Guid id)
        {
            unitOfWork.Services.Delete(id);
            await unitOfWork.SaveChanges();

            return NoContent();
        }

        [Authorize]
        [HttpPut("SubscribeOnService")]
        public async Task<IActionResult> SubcribeOnService(SubscriptionDTO data)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (serviceHandlingService.Subscribe(unitOfWork.Services.GetById(data.ServiceId), user,
                    unitOfWork.Cards.Get(c=>c.CardNumber.Equals(data.CardNumber)).First(), operationService))
                {
                    await unitOfWork.SaveChanges();

                    return NoContent();
                }
            }
            return Conflict(ModelState);
        }

        [Authorize]
        [HttpPut("UnsubscribeFromService/{id}")]
        public async Task<IActionResult> UnsubscribeFromService(Guid id)
        {
            var user = await userManager.GetUserAsync(User);

            if (serviceHandlingService.Unsubscribe(unitOfWork.Services.GetById(id), user))
            {
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return BadRequest(id);
        }
    }
}