using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Core.QueryParams;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class CardService : ICardService
    {
        private readonly IRepository<Card> repository;
        private readonly IMapper mapper;
        private readonly IGeneratorService generatorService;

        public CardService(IRepository<Card> repository, IMapper mapper, IGeneratorService generatorService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.generatorService = generatorService;
        }

        public async Task<Card> OrderCard(CardDTO card, UserProfile currentUser)
        {
            var record = mapper.Map<Card>(card);
            record.CardNumber = generatorService.GetGeneratedCardNumber(await repository.GetAll());
            record.Owner = currentUser;

            var cvv2Builder = new StringBuilder();
            for(int i = 0; i < 3; i++)
            {
                cvv2Builder.Append(new Random().Next(10));
            }
            record.CVV2 = cvv2Builder.ToString();

            record.PaymentSystem = new Random().Next(2) == 1 ? "Visa" : "Mastercard";

            record.ExpirationDate = DateTime.UtcNow.AddYears(5);

            await repository.Create(record);
            return record;
        }

        public void DeleteCard(Guid id)
        {
            repository.Delete(id);
        }

        public async Task<Card> GetCard(Guid id)
        {
            return await repository.GetById(id);
        }

        public async Task<PagedList<Card>> GetCards(UserProfile currentUser, CardParams cardParams)
        {
            return await repository.Get(a => a.OwnerId.Equals(currentUser.Id), cardParams);
        }

        public async Task UpdateCard(Guid id, CardDTO card)
        {
            var record = await repository.GetById(id);
            record = mapper.Map(card, record);

            repository.Update(record);
        }
    }
}
