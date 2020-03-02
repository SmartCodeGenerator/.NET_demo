using BlackCaviarBank.Domain.Core;
using System.Collections.Generic;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IGeneratorService
    {
        string GetGeneratedCardNumber(IEnumerable<Card> cardsToCheck);
        string GetGeneratedAccountNumber(IEnumerable<Account> accountsToCheck);
    }
}
