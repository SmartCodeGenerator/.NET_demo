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
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IGeneratorService generatorService;
        private readonly IFinanceAgentService financeAgentService;

        public CardsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IFinanceAgentService financeAgentService, IMapper mapper, IGeneratorService generatorService)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.financeAgentService = financeAgentService;
            this.mapper = mapper;
            this.generatorService = generatorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            var user = await userManager.GetUserAsync(User);

            return Ok(unitOfWork.Cards.Get(c => c.OwnerId == Guid.Parse(user.Id)));
        }

        [HttpGet("{id}")]
        public IActionResult GetCard(Guid id)
        {
            return Ok(unitOfWork.Cards.GetById(id));
        }

        [HttpPost("BookCard")]
        public async Task<IActionResult> BookCard(CardDTO data)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var card = financeAgentService.GetCardFromData(data, user, generatorService, unitOfWork.Cards.GetAll().ToList(), mapper);

                unitOfWork.Cards.Create(card);

                await unitOfWork.SaveChanges();

                return CreatedAtAction(nameof(GetCard), new { card.CardId }, card);
            }
            return Conflict(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCard(CardDTO data, Guid id)
        {
            if (ModelState.IsValid)
            {
                var card = financeAgentService.GetUpdatedCard(unitOfWork.Cards.GetById(id), data, mapper);

                unitOfWork.Cards.Update(card);

                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(Guid id)
        {
            unitOfWork.Cards.Delete(id);

            await unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}