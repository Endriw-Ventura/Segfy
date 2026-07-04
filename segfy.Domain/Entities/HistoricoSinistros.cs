using segfy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace segfy.Domain.Entities
{
    public class HistoricoSinistros
    {
        public int Id { get; private set; }
        public int SinistroId { get; private set; }
        public StatusSinistro? StatusAnterior { get; private set; }
        public StatusSinistro StatusNovo { get; private set; }
        public string? Observacao { get; private set; }
        public DateTime CriadoEm { get; private set; }

        public HistoricoSinistros(
            int sinistroId,
            StatusSinistro? statusAnterior,
            StatusSinistro statusNovo,
            string? observacao)
        {;
            SinistroId = sinistroId;
            StatusAnterior = statusAnterior;
            StatusNovo = statusNovo;
            Observacao = observacao;
            CriadoEm = DateTime.Now;
        }
    }
}
