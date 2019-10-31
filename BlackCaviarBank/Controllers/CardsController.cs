using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IGenerator generator;

        public CardsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IMapper mapper, IGenerator generator)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
            this.generator = generator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Card>>> GetCards()
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                return unitOfWork.Cards.GetAllForUser(user).ToList();
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var result = unitOfWork.Cards.GetForUser(user, id);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound($"there is no card with id {id}");
                }
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /BookCard
        ///     {
        ///        "balance": 100000
        ///     }
        ///
        /// </remarks>
        [HttpPost("BookCard")]
        public async Task<ActionResult<Card>> BookCard(CardDTO data)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (data.Balance < 0)
                {
                    ModelState.AddModelError("balanceValue", "Card balance must be greater or equal to 0");
                }

                if (ModelState.IsValid)
                {
                    var card = mapper.Map<Card>(data);

                    card.CardNumber = generator.GetGeneratedCardNumber(unitOfWork.Cards.GetAll().ToList());
                    
                    var expDate = DateTime.Now;
                    expDate = expDate.AddYears(5);
                    card.ExpirationDate = expDate;
                    
                    var rand = new Random();
                    int res = rand.Next(0, 2);
                    card.PaymentSystem = res.Equals(0) ? "Visa" : "Mastercard";

                    var cvv2Builder = new StringBuilder();
                    for(int i = 0; i < 3; i++)
                    {
                        cvv2Builder.Append(rand.Next(10));
                    }
                    card.CVV2 = cvv2Builder.ToString();

                    card.Owner = user;
                    user.Cards.Add(card);

                    unitOfWork.Cards.Create(card);

                    await unitOfWork.Save();

                    return CreatedAtAction(nameof(GetCard), new { id = card.CardId }, card);
                }
                else
                {
                    return Conflict(ModelState);
                }
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /UpdateCard
        ///     {
        ///        "balance": 100000
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<Card>> UpdateCard(CardDTO data, int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (data.Balance < 0)
                {
                    ModelState.AddModelError("balanceValue", "Card balance must be greater or equal to 0");
                }

                if (ModelState.IsValid)
                {
                    var target = unitOfWork.Cards.GetForUser(user, id);

                    target.Balance = data.Balance.Equals(0) ? target.Balance : data.Balance;

                    unitOfWork.Cards.Update(target);

                    await unitOfWork.Save();

                    return CreatedAtAction(nameof(GetCard), new { id = target.CardId }, target);
                }
                else
                {
                    return Conflict(ModelState);
                }
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCard(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var target = unitOfWork.Cards.GetForUser(user, id);
                var num = target.CardNumber;

                unitOfWork.Cards.Delete(id);

                await unitOfWork.Save();

                return Ok($"Card with number {num} has been deleted");
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }
    }
}