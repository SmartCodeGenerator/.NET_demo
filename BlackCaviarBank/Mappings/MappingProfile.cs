using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data;

namespace BlackCaviarBank.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDTO, UserProfile>();
            CreateMap<AccountDTO, Account>();
            CreateMap<CardDTO, Card>();
            CreateMap<ServiceDTO, Service>();
            CreateMap<TransactionDTO, Transaction>();
        }
    }
}
