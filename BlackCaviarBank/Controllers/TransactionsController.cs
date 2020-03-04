using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.UnitOfWork;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly ITransactionService transactionService;

        public TransactionsController(UserManager<UserProfile> userManager, UnitOfWork unitOfWork, ITransactionService transactionService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            return Ok(await transactionService.GetTransactionsForCurrentUser(await userManager.GetUserAsync(User)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            return Ok(await transactionService.GetTransaction(id));
        }

        [HttpPost("MakeTransaction")]
        public async Task<IActionResult> MakeTransaction(TransactionDTO data)
        {
            if (ModelState.IsValid)
            {
                var result = await transactionService.MakeTransaction(data, await userManager.GetUserAsync(User));
                await unitOfWork.SaveChanges();

                return CreatedAtAction(nameof(GetTransaction), new { result.TransactionId }, result);
            }
            return Conflict(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RollbackTransaction(Guid id)
        {
            if (ModelState.IsValid)
            {
                await transactionService.RollbackTransaction(id);
                await unitOfWork.SaveChanges();

                return NoContent();
            }
            return Conflict(ModelState);
        }
    }
}