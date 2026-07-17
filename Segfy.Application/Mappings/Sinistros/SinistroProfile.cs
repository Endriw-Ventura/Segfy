using AutoMapper;
using segfy.Domain.Entities;
using Segfy.Application.Sinistros.Results;
namespace Segfy.Application.Mappings.Sinistros
{
    public class SinistroProfile : Profile
    {
        public SinistroProfile()
        {
            CreateMap<Sinistro, SinistroResult>();
            CreateMap<Sinistro, CreateSinistroResult>();
        }
    }
}
