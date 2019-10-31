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
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IGenerator generator;

        public BankAccountsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IMapper mapper, IGenerator generator)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
            this.generator = generator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Account>>> GetAccounts()
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                return unitOfWork.Accounts.GetAllForUser(user).ToList();
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var result = unitOfWork.Accounts.GetForUser(user, id);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound($"there is no account with id {id}");
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
        ///     POST /CreateAccount
        ///     {
        ///        "name": "JotaroKujoDreams",
        ///        "balance": 100000,
        ///        "interestRate": 0.05
        ///     }
        ///
        /// </remarks>
        [HttpPost("CreateAccount")]
        public async Task<ActionResult<Account>> CreateAccount(AccountDTO data)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (string.IsNullOrEmpty(data.Name))
                {
                    ModelState.AddModelError("name", "Account name must not be empty");
                }
                if (data.Balance < 0)
                {
                    ModelState.AddModelError("balanceValue", "Account balance must be greater or equal to 0");
                }
                if (data.InterestRate < 0 || data.InterestRate > 1)
                {
                    ModelState.AddModelError("interestRateValue", "Account interest rate must be greater or equal to 0 and not greater than 1");
                }

                if (ModelState.IsValid)
                {
                    var account = mapper.Map<Account>(data);

                    account.AccountNumber = generator.GetGeneratedAccountNumber(unitOfWork.Accounts.GetAll().ToList());
                    account.OpeningDate = DateTime.Now;
                    account.Owner = user;
                    user.Accounts.Add(account);

                    unitOfWork.Accounts.Create(account);

                    await unitOfWork.Save();

                    return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
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
        ///     PUT /UpdateAccount
        ///     {
        ///        "name": "JotaroKujoDreams",
        ///        "balance": 100000,
        ///        "interestRate": 0.05
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<Account>> UpdateAccount(AccountDTO data, int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (string.IsNullOrEmpty(data.Name))
                {
                    ModelState.AddModelError("name", "Account name must not be empty");
                }
                if (data.Balance < 0)
                {
                    ModelState.AddModelError("balanceValue", "Account balance must be greater or equal to 0");
                }
                if (data.InterestRate < 0)
                {
                    ModelState.AddModelError("interestRateValue", "Account interest rate must be greater or equal to 0");
                }

                if (ModelState.IsValid)
                {
                    var target = unitOfWork.Accounts.GetForUser(user, id);

                    target.Name = data.Name.Equals("string") ? target.Name : data.Name;
                    target.Balance = data.Balance.Equals(0) ? target.Balance : data.Balance;
                    target.InterestRate = data.InterestRate.Equals(0) ? target.InterestRate : data.InterestRate;

                    unitOfWork.Accounts.Update(target);
                    await unitOfWork.Save();

                    return CreatedAtAction(nameof(GetAccount), new { id = target.AccountId }, target);
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
        public async Task<ActionResult> CloseAccount(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var target = unitOfWork.Accounts.GetForUser(user, id);

                string number = target.AccountNumber;

                unitOfWork.Accounts.Delete(target.AccountId);

                await unitOfWork.Save();

                return Ok($"Account with number {number} has been closed");
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }
    }
}
