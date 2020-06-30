using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Business;
using System;
using System.Collections.Generic;
using Xunit;

namespace BlackCaviarBank.Tests
{
    public class NumberGeneratorServiceTests
    {
        [Fact]
        public void GetGeneratedCardNumber_GeneratedNumberNotContainingInExistingSet()
        {
            var cards = new List<Card>()
            {
                new Card {CardNumber = "1111111111111111" },
                new Card {CardNumber = "1111111111111112" }
            };
            var cardNumbers = new List<string>();
            foreach (var card in cards)
            {
                cardNumbers.Add(card.CardNumber);
            }
            var service = new NumberGeneratorService();

            var cardNumber = service.GetGeneratedCardNumber(cards);

            Assert.Equal(16, cardNumber.Length);
            Assert.DoesNotContain(cardNumber, cardNumbers);
        }

        [Fact]
        public void GetGeneratedCardNumber_ThrowsNullReferenceExceptionWhenArgIsNull()
        {
            var service = new NumberGeneratorService();

            Assert.Throws(new NullReferenceException().GetType(), () => service.GetGeneratedCardNumber(null));
        }

        [Fact]
        public void GetGeneratedAccountNumber_GeneratedNumberNotContainingInExistingSet()
        {
            var accs = new List<Account>()
            {
                new Account {AccountNumber = "11111111111111111111" },
                new Account {AccountNumber = "11111111111111111112" }
            };
            var accNumbers = new List<string>();
            foreach (var acc in accs)
            {
                accNumbers.Add(acc.AccountNumber);
            }
            var service = new NumberGeneratorService();

            var accNumber = service.GetGeneratedAccountNumber(accs);

            Assert.Equal(20, accNumber.Length);
            Assert.DoesNotContain(accNumber, accNumbers);
        }

        [Fact]
        public void GetGeneratedAccountNumber_ThrowsNullReferenceExceptionWhenArgIsNull()
        {
            var service = new NumberGeneratorService();

            Assert.Throws(new NullReferenceException().GetType(), () => service.GetGeneratedAccountNumber(null));
        }
    }
}
