using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface ICardService
    {
        Task<PagedList<Card>> GetCards(UserProfile currentUser, CardParams cardParams);
        Task<Card> GetCard(Guid id);
        Task<Card> OrderCard(CardDTO card, UserProfile currentUser);
        Task UpdateCard(Guid id, CardDTO card);
        void DeleteCard(Guid id);
    }
}
