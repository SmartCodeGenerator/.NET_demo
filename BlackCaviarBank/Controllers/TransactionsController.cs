using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
using BlackCaviarBank.Infrastructure.Data.DTOs;
using BlackCaviarBank.Services.Interfaces;
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
    public class TransactionsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IOperationService operation;

        public TransactionsController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IMapper mapper, IOperationService operation)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
            this.operation = operation;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedTransactionsDTO>> GetTransactions(string from, string to, int page = 1)
        {
            var user = await userManager.GetUserAsync(User);

            if(user != null)
            {
                int pageSize = 5;

                var response = unitOfWork.Transactions.GetAllForUser(user);

                if (!string.IsNullOrEmpty(from))
                {
                    response = response.Where(i => i.From.Contains(from));
                }
                if (!string.IsNullOrEmpty(to))
                {
                    response = response.Where(i => i.To.Contains(to));
                }

                int count = response.Count();
                var items = response.Skip((page - 1) * pageSize).Take(pageSize);

                PageDTO pageDTO = new PageDTO(count, page, pageSize);
                var filter = new FilteredTransactionListDTO { Transactions = response.ToList(), From = from, To = to };
                return Ok(new PaginatedTransactionsDTO { FilteredTransactionList = filter, Page = pageDTO });
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var result = unitOfWork.Transactions.GetForUser(user, id);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound($"there is transaction with id {id}");
                }
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        [HttpPost("MakeTransaction")]
        public async Task<ActionResult<Transaction>> MakeTransaction(TransactionDTO data)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (string.IsNullOrEmpty(data.From))
                {
                    ModelState.AddModelError("from", "Transaction from must not be empty");
                }
                if (string.IsNullOrEmpty(data.To))
                {
                    ModelState.AddModelError("to", "Transaction to must not be empty");
                }
                if (!(data.From.Length == 16 || data.From.Length == 20))
                {
                    ModelState.AddModelError("fromLength", "Transaction from length must be equal to 16 or 20");
                }
                if (!(data.To.Length == 16 || data.To.Length == 20))
                {
                    ModelState.AddModelError("toLength", "Transaction to length must be equal to 16 or 20");
                }
                if (data.Amount <= 0)
                {
                    ModelState.AddModelError("amount", "Transaction amount must be greater than 0");
                }

                if (ModelState.IsValid)
                {
                    var transaction = mapper.Map<Transaction>(data);
                    transaction.Date = DateTime.Now;
                    transaction.Payer = user;
                    user.Transactions.Add(transaction);

                    if (transaction.From.Length.Equals(16))
                    {
                        var from = unitOfWork.Cards.GetByNumberForUser(user, transaction.From);

                        if(from != null)
                        {
                            if (!from.IsBlocked)
                            {
                                if (transaction.To.Length.Equals(16))
                                {
                                    var to = unitOfWork.Cards.GetByNumberForUser(user, transaction.To);

                                    if (to != null)
                                    {
                                        if (!to.IsBlocked)
                                        {
                                            if (operation.ExecuteFromCardToCard(from, to, transaction.Amount))
                                            {
                                                unitOfWork.Transactions.Create(transaction);
                                                await unitOfWork.Save();
                                                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
                                            }
                                            else
                                            {
                                                return BadRequest($"Not enough money on card {from.CardNumber}");
                                            }
                                        }
                                        else
                                        {
                                            return BadRequest($"card {to.CardNumber} is blocked");
                                        }
                                    }
                                    else
                                    {
                                        return NotFound($"There is no card with number {transaction.To}");
                                    }
                                }
                                else
                                {
                                    var to = unitOfWork.Accounts.GetByNumberForUser(user, transaction.To);

                                    if (to != null)
                                    {
                                        if (!to.IsBlocked)
                                        {
                                            if (operation.ExecuteFromCardToAccount(from, to, transaction.Amount))
                                            {
                                                unitOfWork.Transactions.Create(transaction);
                                                await unitOfWork.Save();
                                                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
                                            }
                                            else
                                            {
                                                return BadRequest($"Not enough money on card {from.CardNumber}");
                                            }
                                        }
                                        else
                                        {
                                            return BadRequest($"account {to.AccountNumber} is blocked");
                                        }
                                    }
                                    else
                                    {
                                        return NotFound($"There is no account with number {transaction.To}");
                                    }
                                }
                            }
                            else
                            {
                                return BadRequest($"card {from.CardNumber} is blocked");
                            }
                        }
                        else
                        {
                            return NotFound($"There is no card with number {transaction.From}");
                        }
                    }
                    else
                    {
                        var from = unitOfWork.Accounts.GetByNumberForUser(user, transaction.From);

                        if (from != null)
                        {
                            if (!from.IsBlocked)
                            {
                                if (transaction.To.Length.Equals(16))
                                {
                                    var to = unitOfWork.Cards.GetByNumberForUser(user, transaction.To);

                                    if (to != null)
                                    {
                                        if (!to.IsBlocked)
                                        {
                                            if (operation.ExecuteFromAccountToCard(from, to, transaction.Amount))
                                            {
                                                unitOfWork.Transactions.Create(transaction);
                                                await unitOfWork.Save();
                                                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
                                            }
                                            else
                                            {
                                                return BadRequest($"Not enough money on account {from.AccountNumber}");
                                            }
                                        }
                                        else
                                        {
                                            return BadRequest($"card {to.CardNumber} is blocked");
                                        }
                                    }
                                    else
                                    {
                                        return NotFound($"There is no card with number {transaction.To}");
                                    }
                                }
                                else
                                {
                                    var to = unitOfWork.Accounts.GetByNumberForUser(user, transaction.To);

                                    if (to != null)
                                    {
                                        if (!to.IsBlocked)
                                        {
                                            if (operation.ExecuteFromAccountToAccount(from, to, transaction.Amount))
                                            {
                                                unitOfWork.Transactions.Create(transaction);
                                                await unitOfWork.Save();
                                                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
                                            }
                                            else
                                            {
                                                return BadRequest($"Not enough money on account {from.AccountNumber}");
                                            }
                                        }
                                        else
                                        {
                                            return BadRequest($"account {to.AccountNumber} is blocked");
                                        }
                                    }
                                    else
                                    {
                                        return NotFound($"There is no account with number {transaction.To}");
                                    }
                                }
                            }
                            else
                            {
                                return BadRequest($"account {from.AccountNumber} is blocked");
                            }
                        }
                        else
                        {
                            return NotFound($"There is no account with number {transaction.From}");
                        }
                    }
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
        public async Task<ActionResult> CancelTransaction(int id)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var transaction = unitOfWork.Transactions.GetForUser(user, id);

                if (transaction != null)
                {
                    if (transaction.From.Length.Equals(16))
                    {
                        if (transaction.To.Length.Equals(16))
                        {
                            if (operation.RefundFromCardToCard(unitOfWork.Cards.GetByNumberForUser(user, transaction.To), unitOfWork.Cards.GetByNumberForUser(user, transaction.From), transaction.Amount))
                            {
                                unitOfWork.Transactions.Delete(id);
                                await unitOfWork.Save();
                                return Ok($"transaction with id {id} from {transaction.Date} has been cancelled");
                            }
                            else
                            {
                                return BadRequest($"Not enough money on card with number {transaction.To}");
                            }
                        }
                        else
                        {
                            if (operation.RefundFromAccountToCard(unitOfWork.Accounts.GetByNumberForUser(user, transaction.To), unitOfWork.Cards.GetByNumberForUser(user, transaction.From), transaction.Amount))
                            {
                                unitOfWork.Transactions.Delete(id);
                                await unitOfWork.Save();
                                return Ok($"transaction with id {id} from {transaction.Date} has been cancelled");
                            }
                            else
                            {
                                return BadRequest($"Not enough money on account with number {transaction.To}");
                            }
                        }
                    }
                    else
                    {
                        if (transaction.To.Length.Equals(16))
                        {
                            if (operation.RefundFromCardToAccount(unitOfWork.Cards.GetByNumberForUser(user, transaction.To), unitOfWork.Accounts.GetByNumberForUser(user, transaction.From), transaction.Amount))
                            {
                                unitOfWork.Transactions.Delete(id);
                                await unitOfWork.Save();
                                return Ok($"transaction with id {id} from {transaction.Date} has been cancelled");
                            }
                            else
                            {
                                return BadRequest($"Not enough money on card with number {transaction.To}");
                            }
                        }
                        else
                        {
                            if (operation.RefundFromAccountToAccount(unitOfWork.Accounts.GetByNumberForUser(user, transaction.To), unitOfWork.Accounts.GetByNumberForUser(user, transaction.From), transaction.Amount))
                            {
                                unitOfWork.Transactions.Delete(id);
                                await unitOfWork.Save();
                                return Ok($"transaction with id {id} from {transaction.Date} has been cancelled");
                            }
                            else
                            {
                                return BadRequest($"Not enough money on account with number {transaction.To}");
                            }
                        }
                    }
                }
                else
                {
                    return NotFound($"There is no transaction with id {id}");
                }
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }
    }
}