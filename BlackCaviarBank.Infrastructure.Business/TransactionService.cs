﻿using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction> transactionRepository;
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<Card> cardRepository;
        private readonly IMapper mapper;

        public TransactionService(IRepository<Transaction> transactionRepository, IRepository<Account> accountRepository, IRepository<Card> cardRepository, IMapper mapper)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.cardRepository = cardRepository;
            this.mapper = mapper;
        }

        public async Task<Transaction> GetTransaction(Guid id)
        {
            return await transactionRepository.GetById(id);
        }

        public async Task<PagedList<Transaction>> GetTransactionsForCurrentUser(UserProfile user, TransactionParams transactionParams)
        {
            var result = await transactionRepository.GetAll();
            var items = new List<Transaction>();
            int count = 0;
            if (result != null)
            {
                var list = result.Where(t => t.PayerId.Equals(user.Id)).ToList();
                count = list.Count();
                items = list.Skip((transactionParams.PageNumber - 1) * transactionParams.PageSize).Take(transactionParams.PageSize).ToList();
            }

            return new PagedList<Transaction>(items, count, transactionParams.PageNumber, transactionParams.PageSize);
        }

        public async Task<Transaction> MakeTransaction(TransactionDTO transaction, UserProfile payer)
        {
            var record = mapper.Map<Transaction>(transaction);
            record.Date = DateTime.UtcNow;
            record.Payer = payer;

            bool isSucceeded = false;

            Card payingCard = transaction.From.Length == 16 ? (await cardRepository.GetByCriterion(c => c.CardNumber.Equals(transaction.From))) : null;
            Account payingAccount = transaction.From.Length == 20 ? (await accountRepository.GetByCriterion(a => a.AccountNumber.Equals(transaction.From))) : null;
            Card receivingCard = transaction.To.Length == 16 ? (await cardRepository.GetByCriterion(c => c.CardNumber.Equals(transaction.To))) : null;
            Account receivingAccount = transaction.To.Length == 20 ? (await accountRepository.GetByCriterion(a => a.AccountNumber.Equals(transaction.To))) : null;

            if (payingCard != null && receivingCard != null)
            {
                if (!payingCard.IsBlocked && payingCard.Balance > transaction.Amount && payingCard.OwnerId.Equals(payer.Id))
                {
                    payingCard.Balance -= transaction.Amount;
                    receivingCard.Balance += transaction.Amount;
                    isSucceeded = true;
                    cardRepository.Update(payingCard);
                    cardRepository.Update(receivingCard);
                }
            }
            else if (payingCard != null && receivingAccount != null)
            {
                if (!payingCard.IsBlocked && payingCard.Balance > transaction.Amount && payingCard.OwnerId.Equals(payer.Id))
                {
                    payingCard.Balance -= transaction.Amount;
                    receivingAccount.Balance += transaction.Amount;
                    isSucceeded = true;
                    cardRepository.Update(payingCard);
                    accountRepository.Update(receivingAccount);
                }
            }
            else if (payingAccount != null && receivingCard != null)
            {
                if (!payingAccount.IsBlocked && payingAccount.Balance > transaction.Amount && payingAccount.OwnerId.Equals(payer.Id))
                {
                    payingAccount.Balance -= transaction.Amount;
                    receivingCard.Balance += transaction.Amount;
                    isSucceeded = true;
                    accountRepository.Update(payingAccount);
                    cardRepository.Update(receivingCard);
                }
            }
            else if (payingAccount != null && receivingAccount != null)
            {
                if (!payingAccount.IsBlocked && payingAccount.Balance > transaction.Amount && payingAccount.OwnerId.Equals(payer.Id))
                {
                    payingAccount.Balance -= transaction.Amount;
                    receivingAccount.Balance += transaction.Amount;
                    isSucceeded = true;
                    accountRepository.Update(payingAccount);
                    accountRepository.Update(receivingAccount);
                }
            }

            if (isSucceeded)
            {
                await transactionRepository.Create(record);
                return record;
            }
            return null;
        }

        public async Task RollbackTransaction(Guid id)
        {
            var transaction = await transactionRepository.GetById(id);

            Card payingCard = transaction.To.Length == 16 ? (await cardRepository.GetByCriterion(c => c.CardNumber.Equals(transaction.To))) : null;
            Account payingAccount = transaction.To.Length == 20 ? (await accountRepository.GetByCriterion(a => a.AccountNumber.Equals(transaction.To))) : null;
            Card receivingCard = transaction.From.Length == 16 ? (await cardRepository.GetByCriterion(c => c.CardNumber.Equals(transaction.From))) : null;
            Account receivingAccount = transaction.From.Length == 20 ? (await accountRepository.GetByCriterion(a => a.AccountNumber.Equals(transaction.From))) : null;

            if (payingCard != null && receivingCard != null)
            {
                payingCard.Balance -= transaction.Amount;
                receivingCard.Balance += transaction.Amount;
                cardRepository.Update(payingCard);
                cardRepository.Update(receivingCard);
            }
            else if (payingCard != null && receivingAccount != null)
            {
                payingCard.Balance -= transaction.Amount;
                receivingAccount.Balance += transaction.Amount;
                cardRepository.Update(payingCard);
                accountRepository.Update(receivingAccount);
            }
            else if (payingAccount != null && receivingCard != null)
            {
                payingAccount.Balance -= transaction.Amount;
                receivingCard.Balance += transaction.Amount;
                accountRepository.Update(payingAccount);
                cardRepository.Update(receivingCard);
            }
            else if (payingAccount != null && receivingAccount != null)
            {
                payingAccount.Balance -= transaction.Amount;
                receivingAccount.Balance += transaction.Amount;
                accountRepository.Update(payingAccount);
                accountRepository.Update(receivingAccount);
            }

            transactionRepository.Delete(id);
        }
    }
}
