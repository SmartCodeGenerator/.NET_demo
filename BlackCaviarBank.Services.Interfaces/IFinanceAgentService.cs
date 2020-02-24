using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System.Collections.Generic;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IFinanceAgentService
    {
        public Account GetAccountFromData(AccountDTO data, UserProfile owner, IGeneratorService generatorService, List<Account> accountsToCheck, IMapper mapper);
        public Account GetUpdatedAccount(Account target, AccountDTO data, IMapper mapper);
        public Card GetCardFromData(CardDTO data, UserProfile owner, IGeneratorService generatorService, List<Card> cardsToCheck, IMapper mapper);
        public Card GetUpdatedCard(Card target, CardDTO data, IMapper mapper);
    }
}
