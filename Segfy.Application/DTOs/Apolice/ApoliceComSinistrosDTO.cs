using segfy.Domain.Enums;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.DTOs.Apolice
{
    public class ApoliceComSinistrosDTO
    {
        public int Id { get; set; }
        public string NumeroApolice { get; set; }
        public string NomeSegurado { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public StatusApolice Status { get; set; }
        public List<GetSinistroDTO> Sinistros { get; set; } = new List<GetSinistroDTO>();
        public Ramo Ramo { get; set; }
    }
}
