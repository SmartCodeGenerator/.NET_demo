﻿using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly ICardService cardService;

        public CardsController(UserManager<UserProfile> userManager, UnitOfWork unitOfWork, ICardService cardService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.cardService = cardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCards([FromQuery] CardParams cardParams)
        {
            var result = await cardService.GetCards(await userManager.FindByNameAsync(User.Identity.Name), cardParams);

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCard(Guid id)
        {
            return Ok(await cardService.GetCard(id));
        }

        [HttpPost("OrderCard")]
        public async Task<IActionResult> OrderCard(CardDTO data)
        {
            if (ModelState.IsValid)
            {
                var result = await cardService.OrderCard(data, await userManager.FindByNameAsync(User.Identity.Name));
                await unitOfWork.SaveChanges();

                return CreatedAtAction(nameof(GetCard),
                    new { id = result.CardId },
                    new
                    {
                        cardId = result.CardId,
                        cardNumber = result.CardNumber,
                        expirationDate = result.ExpirationDate,
                        paymentSystem = result.PaymentSystem,
                        cvv2 = result.CVV2,
                        balance = result.Balance,
                        isBlocked = result.IsBlocked,
                        ownerId = result.OwnerId
                    });
            }
            return Conflict(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCard(CardDTO data, Guid id)
        {
            if (ModelState.IsValid)
            {
                await cardService.UpdateCard(id, data);
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(Guid id)
        {
            cardService.DeleteCard(id);
            await unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}