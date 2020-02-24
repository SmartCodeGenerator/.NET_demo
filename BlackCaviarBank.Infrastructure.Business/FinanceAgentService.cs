using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class FinanceAgentService : IFinanceAgentService
    {
        public Account GetAccountFromData(AccountDTO data, UserProfile owner, IGeneratorService generatorService, List<Account> accountsToCheck, IMapper mapper)
        {
            Account account = mapper.Map<Account>(data);

            account.AccountNumber = generatorService.GetGeneratedAccountNumber(accountsToCheck);
            account.OpeningDate = DateTime.UtcNow;
            account.Owner = owner;

            owner.Accounts.Add(account);

            return account;
        }

        public Card GetCardFromData(CardDTO data, UserProfile owner, IGeneratorService generatorService, List<Card> cardsToCheck, IMapper mapper)
        {
            Card card = mapper.Map<Card>(data);

            card.CardNumber = generatorService.GetGeneratedCardNumber(cardsToCheck);

            var expDate = DateTime.UtcNow;
            expDate = expDate.AddYears(5);
            card.ExpirationDate = expDate;

            var rand = new Random();
            int res = rand.Next(0, 2);
            card.PaymentSystem = res.Equals(0) ? "Visa" : "Mastercard";

            var cvv2Builder = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                cvv2Builder.Append(rand.Next(10));
            }
            card.CVV2 = cvv2Builder.ToString();
            card.Owner = owner;

            owner.Cards.Add(card);

            return card;
        }

        public Account GetUpdatedAccount(Account target, AccountDTO data, IMapper mapper)
        {
            mapper.Map(data, target);

            return target;
        }

        public Card GetUpdatedCard(Card target, CardDTO data, IMapper mapper)
        {
            mapper.Map(data, target);

            return target;
        }
    }
}
