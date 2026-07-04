using segfy.Domain.Entities;
using segfy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.DTOs.Sinistro
{
    public class CreateSinistroDTO
    {
        public int Id { get; set; }
        public string NumeroSinistro { get; set; }
        public DateTime DataSinistro { get; set; }
        public string Descricao { get; set; }
        public decimal ValorSolicitado { get; set; }
        public int ApoliceId { get; set; }
    }
}
