using segfy.Domain.Enums;
using Segfy.Application.DTOs.Apolice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.DTOs.Sinistro
{
    public class GetSinistroDTO
    {
        public int Id { get; set; }
        public string NumeroSinistro { get; set; }
        public DateTime DataSinistro { get; set; }
        public string Descricao { get; set; }
        public decimal ValorSolicitado { get; set; }
        public decimal? ValorAprovado { get; set; }
        public string? MotivoNegativa { get; set; }
        public StatusSinistro Status { get; set; }
    }
}
