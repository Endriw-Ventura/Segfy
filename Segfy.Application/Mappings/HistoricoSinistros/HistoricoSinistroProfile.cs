using AutoMapper;
using segfy.Domain.Entities;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Mappings
{
    public class HistoricoSinistroProfile : Profile
    {
        public HistoricoSinistroProfile()
        {
            CreateMap<HistoricoSinistros, GetHistoricoSinistroResult>();
        }
    }
}
