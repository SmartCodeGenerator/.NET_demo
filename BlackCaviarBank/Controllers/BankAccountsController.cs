using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IAccountService accountService;

        public BankAccountsController(UserManager<UserProfile> userManager, UnitOfWork unitOfWork, IAccountService accountService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts([FromQuery] BankAccountParams bankAccountParams)
        {
            var result = await accountService.GetAccounts(await userManager.FindByNameAsync(User.Identity.Name), bankAccountParams);

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
        public async Task<IActionResult> GetAccount(Guid id)
        {
            return Ok(await accountService.GetAccount(id));
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(AccountDTO data)
        {
            if (ModelState.IsValid)
            {
                var result = await accountService.CreateAccount(data, await userManager.FindByNameAsync(User.Identity.Name));
                await unitOfWork.SaveChanges();

                return CreatedAtAction(nameof(GetAccount),
                    new { id = result.AccountId },
                    new
                    {
                        id = result.AccountId,
                        number = result.AccountNumber,
                        name = result.Name,
                        openingDate = result.OpeningDate,
                        balance = result.Balance,
                        interestRate = result.InterestRate,
                        isBlocked = result.IsBlocked,
                        ownerId = result.OwnerId
                    });
            }
            return Conflict(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(AccountDTO data, Guid id)
        {
            if (ModelState.IsValid)
            {
                await accountService.UpdateAccount(id, data);
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            accountService.DeleteAccount(id);
            await unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}
