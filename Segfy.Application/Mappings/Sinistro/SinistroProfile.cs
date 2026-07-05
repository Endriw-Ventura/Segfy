using AutoMapper;
using Segfy.Application.DTOs.Sinistro;
using DomainSinistro = segfy.Domain.Entities.Sinistro;

namespace Segfy.Application.Mappings.Sinistro
{
    public class SinistroProfile : Profile
    {
        public SinistroProfile()
        {
            CreateMap<DomainSinistro, SinistroDTO>();
            CreateMap<DomainSinistro, CreateSinistroDTO>();
            CreateMap<DomainSinistro, GetSinistroDTO>();
        }
    }
}
