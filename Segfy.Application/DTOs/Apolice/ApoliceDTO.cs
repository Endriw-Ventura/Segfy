using segfy.Domain.Enums;
using Segfy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.DTOs.Apolice
{
    public class ApoliceDTO
    {
        public int Id { get; set; }
        public string NumeroApolice { get; set; }
        public string NomeSegurado { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public StatusApolice Status { get; set; }
        public Ramo Ramo { get; set; }
    }
}
