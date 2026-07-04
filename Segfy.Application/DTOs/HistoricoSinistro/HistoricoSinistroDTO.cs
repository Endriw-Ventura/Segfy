using segfy.Domain.Enums;
using Segfy.Application.DTOs.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.DTOs.HistoricoSinistro
{
    public class HistoricoSinistroDTO
    {
        public int Id { get; set; }
        public StatusSinistro? StatusAnterior { get; set; }
        public StatusSinistro StatusNovo { get; set; }
        public string? Observacao { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
