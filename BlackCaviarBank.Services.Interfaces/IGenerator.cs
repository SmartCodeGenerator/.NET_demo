using BlackCaviarBank.Domain.Core;
using System.Collections.Generic;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IGenerator
    {
        string GetGeneratedCardNumber(List<Card> cardsToCheck);
        string GetGeneratedAccountNumber(List<Account> accountsToCheck);
    }
}
