using AutoMapper;
using segfy.Domain.Entities;
using Segfy.Application.Apolices.Results;
namespace Segfy.Application.Mappings.Apolices
{
    public class ApoliceProfile : Profile
    {
        public ApoliceProfile()
        {
            CreateMap<Apolice, CreateApoliceResult>();
            CreateMap<Apolice, GetApoliceWithSinistrosQueryResult>();
            CreateMap<Apolice, ApoliceResult>();
        }
    }
}
