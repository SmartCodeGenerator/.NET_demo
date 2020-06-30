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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionParams transactionParams)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var result = await transactionService.GetTransactionsForCurrentUser(user, transactionParams);

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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            return Ok(await transactionService.GetTransaction(id));
        }

        [Authorize]
        [HttpPost("MakeTransaction")]
        public async Task<IActionResult> MakeTransaction(TransactionDTO data)
        {
            if (ModelState.IsValid)
            {
                var result = await transactionService.MakeTransaction(data, await userManager.FindByNameAsync(User.Identity.Name));
                await unitOfWork.SaveChanges();

                if (result != null)
                {
                    return CreatedAtAction(nameof(GetTransaction),
                        new { id = result.TransactionId },
                        new
                        { 
                            id = result.TransactionId,
                            from = result.From,
                            to = result.To,
                            amount = result.Amount,
                            date = result.Date,
                            payerId = result.PayerId
                        });
                }
                else
                {
                    return BadRequest(data);
                }
            }
            return Conflict(ModelState);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RollbackTransaction(Guid id)
        {
            await transactionService.RollbackTransaction(id);
            await unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}