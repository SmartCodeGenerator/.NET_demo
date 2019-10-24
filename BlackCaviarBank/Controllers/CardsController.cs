using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
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
    public class CardsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CardsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns a collection of user`s cards.
        /// </summary>
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

        /// <summary>
        /// Returns a specific user`s card.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                return unitOfWork.Cards.GetForUser(user, id);
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        /// <summary>
        /// Creates a specific user`s card.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /BookCard
        ///     {
        ///        "expirationDate": "2012-04-23T18:25:43.511Z",
        ///        "paymentSystem": "Visa",
        ///        "CVV2: "000",
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
                if (string.IsNullOrEmpty(data.PaymentSystem))
                {
                    ModelState.AddModelError("paySys", "Card payment system must not be empty");
                }
                if (string.IsNullOrEmpty(data.CVV2))
                {
                    ModelState.AddModelError("cvv2", "Card CVV2 must not be empty");
                }
                if (data.CVV2.Length != 3)
                {
                    ModelState.AddModelError("cvv2Length", "Card cvv2 must contain 3 numbers");
                }
                if (data.Balance < 0)
                {
                    ModelState.AddModelError("balanceValue", "Card balance must be greater or equal to 0");
                }

                if (ModelState.IsValid)
                {
                    var card = mapper.Map<Card>(data);

                    var gen = new Random();
                    var cardNum = new byte[16];
                    gen.NextBytes(cardNum);

                    card.CardNumber = cardNum.ToString();
                    card.Owner = user;

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

        /// <summary>
        /// Updates a specific user`s card.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /UpdateCard
        ///     {
        ///        "expirationDate": "2012-04-23T18:25:43.511Z",
        ///        "paymentSystem": "Visa",
        ///        "cVV2: "000",
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
                if (string.IsNullOrEmpty(data.PaymentSystem))
                {
                    ModelState.AddModelError("paySys", "Card payment system must not be empty");
                }
                if (string.IsNullOrEmpty(data.CVV2))
                {
                    ModelState.AddModelError("cvv2", "Card CVV2 must not be empty");
                }
                if (data.CVV2.Length != 3)
                {
                    ModelState.AddModelError("cvv2Length", "Card cvv2 must contain 3 numbers");
                }
                if (data.Balance < 0)
                {
                    ModelState.AddModelError("balanceValue", "Card balance must be greater or equal to 0");
                }

                if (ModelState.IsValid)
                {
                    var target = unitOfWork.Cards.GetForUser(user, id);

                    

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

        /// <summary>
        /// Deletes a specific user`s card.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCard(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var target = unitOfWork.Cards.GetForUser(user, id);

                unitOfWork.Cards.Delete(target.CardId);

                await unitOfWork.Save();

                return Ok($"Card with id {id} has been deleted");
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }
    }
}