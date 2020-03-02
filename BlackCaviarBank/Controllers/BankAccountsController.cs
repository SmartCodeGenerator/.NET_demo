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
        public async Task<IActionResult> GetAccounts()
        {
            return Ok(accountService.GetAccounts(await userManager.GetUserAsync(User)));
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
                await accountService.CreateAccount(data, await userManager.GetUserAsync(User));
                await unitOfWork.SaveChanges();

                return CreatedAtAction(nameof(GetAccount),
                    new { unitOfWork.Accounts.Get(a => a.Name.Equals(data.Name)).FirstOrDefault().AccountId },
                    unitOfWork.Accounts.Get(a => a.Name.Equals(data.Name)).FirstOrDefault());
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
