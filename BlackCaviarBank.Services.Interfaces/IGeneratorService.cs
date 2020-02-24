using BlackCaviarBank.Domain.Core;
using System.Collections.Generic;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IGeneratorService
    {
        string GetGeneratedCardNumber(List<Card> cardsToCheck);
        string GetGeneratedAccountNumber(List<Account> accountsToCheck);
    }
}
