using segfy.Domain.Enums;
namespace segfy.Domain.Entities
{
    public class HistoricoSinistros
    {
        public int Id { get; private set; }
        public int SinistroId { get; private set; }
        public Sinistro Sinistro { get; private set; }
        public StatusSinistro? StatusAnterior { get; private set; }
        public StatusSinistro StatusNovo { get; private set; }
        public string? Observacao { get; private set; }
        public DateTime CriadoEm { get; private set; }

        protected HistoricoSinistros() { }

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
