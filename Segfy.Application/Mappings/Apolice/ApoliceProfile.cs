using AutoMapper;
using Segfy.Application.DTOs.Apolice;
using DomainApolice = segfy.Domain.Entities.Apolice;

namespace Segfy.Application.Mappings.Apolice
{
    public class ApoliceProfile : Profile
    {
        public ApoliceProfile()
        {
            CreateMap<DomainApolice, ApoliceDTO>();
            CreateMap<DomainApolice, CreateApoliceDTO>();
            CreateMap<DomainApolice, ApoliceComSinistrosDTO>();
        }
    }
}
