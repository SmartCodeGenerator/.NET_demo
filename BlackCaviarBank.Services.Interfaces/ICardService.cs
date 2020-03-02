using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface ICardService
    {
        IEnumerable<Card> GetCards(UserProfile currentUser);
        Task<Card> GetCard(Guid id);
        Task OrderCard(CardDTO card, UserProfile currentUser);
        Task UpdateCard(Guid id, CardDTO card);
        void DeleteCard(Guid id);
    }
}
