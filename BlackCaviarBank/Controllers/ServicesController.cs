﻿using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly ISubscriptionService subscriptionService;

        public ServicesController(UserManager<UserProfile> userManager, UnitOfWork unitOfWork, ISubscriptionService subscriptionService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.subscriptionService = subscriptionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllServices([FromQuery] ServiceParams serviceParams)
        {
            var result = await subscriptionService.GetServices(serviceParams);

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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(Guid id)
        {
            return Ok(await subscriptionService.GetService(id));
        }

        [Authorize]
        [HttpGet("UserSubscriptions")]
        public async Task<IActionResult> GetUserSubscriptions([FromQuery] ServiceParams serviceParams)
        {
            var result = await subscriptionService.GetUserSubscriptions(await userManager.FindByNameAsync(User.Identity.Name), serviceParams);

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

        [Authorize(Roles = "admin")]
        [HttpPost("RegisterService")]
        public async Task<IActionResult> CreateService(ServiceDTO data)
        {
            if (ModelState.IsValid)
            {
                var result = await subscriptionService.CreateService(data);
                await unitOfWork.SaveChanges();

                if (result != null)
                {
                    return CreatedAtAction(nameof(GetService), new { id = result.ServiceId }, result);
                }
                else
                {
                    return BadRequest(data);
                }
            }
            return Conflict(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(ServiceDTO data, Guid id)
        {
            if (ModelState.IsValid)
            {
                await subscriptionService.UpdateService(data, id);
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveService(Guid id)
        {
            subscriptionService.RemoveService(id);
            await unitOfWork.SaveChanges();

            return NoContent();
        }

        [Authorize]
        [HttpPut("SubscribeOnService")]
        public async Task<IActionResult> SubcribeOnService(SubscriptionDTO data)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var card = await unitOfWork.Cards.GetByNumber(data.CardNumber);
                await subscriptionService.SubscribeOnService(user, card, data.ServiceId);
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [Authorize]
        [HttpPut("UnsubscribeFromService/{id}")]
        public async Task<IActionResult> UnsubscribeFromService(Guid id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            await subscriptionService.UnsubscribeFromService(user, id);
            await unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}