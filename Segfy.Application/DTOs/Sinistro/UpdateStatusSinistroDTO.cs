using segfy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.DTOs.Sinistro
{
    public class UpdateStatusSinistroDTO
    {
        public StatusSinistro Status { get; set; }
        public string? MotivoNegativa { get; set; }
        public decimal? ValorAprovado { get; set; }
    }
}
