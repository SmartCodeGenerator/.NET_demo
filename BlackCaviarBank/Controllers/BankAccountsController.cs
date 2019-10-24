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
    public class BankAccountsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BankAccountsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns a collection of user`s accounts.
        /// </summary>
        [HttpGet("GetAccounts")]
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

        /// <summary>
        /// Returns a specific user`s account.
        /// </summary>
        [HttpGet("GetAccount/{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                return unitOfWork.Accounts.GetForUser(user, id);
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        /// <summary>
        /// Creates a specific user`s account.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /CreateAccount
        ///     {
        ///        "accountNumber": "00000000000000000000",
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
                if (string.IsNullOrEmpty(data.AccountNumber))
                {
                    ModelState.AddModelError("accountNumber", "Account number must not be empty");
                }
                if (string.IsNullOrEmpty(data.Name))
                {
                    ModelState.AddModelError("name", "Account name must not be empty");
                }
                if (data.AccountNumber.Length != 20)
                {
                    ModelState.AddModelError("nameLength", "Account name must contain 20 numbers");
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
                    var account = mapper.Map<Account>(data);
                    account.OpeningDate = DateTime.Now;
                    account.Owner = user;

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

        /// <summary>
        /// Updates a specific user`s account.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /UpdateAccount
        ///     {
        ///        "accountNumber": "00000000000000000000",
        ///        "name": "JotaroKujoDreams",
        ///        "balance": 100000,
        ///        "interestRate": 0.05
        ///     }
        ///
        /// </remarks>
        [HttpPut("UpdateAccount/{id}")]
        public async Task<ActionResult<Account>> UpdateAccount(AccountDTO data, int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (string.IsNullOrEmpty(data.AccountNumber))
                {
                    ModelState.AddModelError("accountNumber", "Account number must not be empty");
                }
                if (string.IsNullOrEmpty(data.Name))
                {
                    ModelState.AddModelError("name", "Account name must not be empty");
                }
                if (data.AccountNumber.Length != 20)
                {
                    ModelState.AddModelError("nameLength", "Account name must contain 20 numbers");
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

                    target = mapper.Map<Account>(data);

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


        /// <summary>
        /// Deletes a specific user`s account.
        /// </summary>
        [HttpDelete("CloseAccount/{id}")]
        public async Task<ActionResult> CloseAccount(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var target = unitOfWork.Accounts.GetForUser(user, id);

                unitOfWork.Accounts.Delete(target.AccountId);

                await unitOfWork.Save();

                return Ok($"Account with id {id} has been closed");
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }
    }
}
