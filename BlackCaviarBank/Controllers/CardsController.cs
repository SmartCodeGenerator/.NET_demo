using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
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
        public async Task<IActionResult> GetCards()
        {
            return Ok(cardService.GetCards(await userManager.GetUserAsync(User)));
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
                await cardService.OrderCard(data, await userManager.GetUserAsync(User));
                await unitOfWork.SaveChanges();

                var createdCards = cardService.GetCards(await userManager.GetUserAsync(User));

                return CreatedAtAction(nameof(GetCard), new { createdCards.Last().CardId }, createdCards.Last());
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