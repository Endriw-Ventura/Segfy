using segfy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.DTOs.Apolice
{
    public class CreateApoliceDTO
    {
        public string NumeroApolice { get; set; }
        public string NomeSegurado { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
