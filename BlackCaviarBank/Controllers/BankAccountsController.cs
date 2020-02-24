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
    public class BankAccountsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IGeneratorService generatorService;
        private readonly IFinanceAgentService financeAgentService;

        public BankAccountsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IFinanceAgentService financeAgentService, IMapper mapper, IGeneratorService generatorService)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.financeAgentService = financeAgentService;
            this.mapper = mapper;
            this.generatorService = generatorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var user = await userManager.GetUserAsync(User);

            return Ok(unitOfWork.Accounts.Get(a => a.OwnerId == Guid.Parse(user.Id)));
        }

        [HttpGet("{id}")]
        public IActionResult GetAccount(Guid id)
        {
            return Ok(unitOfWork.Accounts.GetById(id));
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(AccountDTO data)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var account = financeAgentService.GetAccountFromData(data, user, generatorService, unitOfWork.Accounts.GetAll().ToList(), mapper);

                unitOfWork.Accounts.Create(account);

                await unitOfWork.SaveChanges();

                return CreatedAtAction(nameof(GetAccount), new { account.AccountId }, account);
            }
            return Conflict(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(AccountDTO data, Guid id)
        {
            if (ModelState.IsValid)
            {
                var account = financeAgentService.GetUpdatedAccount(unitOfWork.Accounts.GetById(id), data, mapper);

                unitOfWork.Accounts.Update(account);

                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            unitOfWork.Accounts.Delete(id);

            await unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}
