using AutoMapper;
using segfy.Domain.Entities;
using Segfy.Application.DTOs.HistoricoSinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.Mappings
{
    public class HistoricoSinistroProfile : Profile
    {
        public HistoricoSinistroProfile()
        {
            CreateMap<HistoricoSinistros, HistoricoSinistroDTO>();
        }
    }
}
