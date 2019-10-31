using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class NumberGenerator : IGenerator
    {
        public string GetGeneratedCardNumber(List<Card> cardsToCheck)
        {
            var rand = new Random();
            StringBuilder numBuilder = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                numBuilder.Append(rand.Next(10));
            }

            foreach(var card in cardsToCheck)
            {
                if (card.CardNumber.Equals(numBuilder.ToString()))
                {
                    GetGeneratedCardNumber(cardsToCheck);
                }
            }

            return numBuilder.ToString();
        }

        public string GetGeneratedAccountNumber(List<Account> accountsToCheck)
        {
            var rand = new Random();
            StringBuilder numBuilder = new StringBuilder();
            for (int i = 0; i < 20; i++)
            {
                numBuilder.Append(rand.Next(10));
            }

            foreach (var account in accountsToCheck)
            {
                if (account.AccountNumber.Equals(numBuilder.ToString()))
                {
                    GetGeneratedAccountNumber(accountsToCheck);
                }
            }

            return numBuilder.ToString();
        }
    }
}
