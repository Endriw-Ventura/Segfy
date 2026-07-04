using segfy.Domain.Enums;

namespace segfy.Domain.Entities
{
    public class Apolice(string numeroApolice, string nomeSegurado, DateTime dataInicio, DateTime dataFim)
    {
        public int Id { get; set; }
        public string NumeroApolice { get; private set; } = numeroApolice;
        public string NomeSegurado { get; private set; } = nomeSegurado;
        public DateTime DataInicio { get; private set; } = dataInicio;
        public DateTime DataFim { get; private set; } = dataFim;
        public StatusApolice Status { get; private set; } = StatusApolice.ATIVA;

        private readonly List<Sinistro> _sinistros = [];
        public IReadOnlyCollection<Sinistro> Sinistros => _sinistros.AsReadOnly();

        public bool IsAtiva()
        {
            return Status == StatusApolice.ATIVA;
        }

        public void Cancelar()
        {
            Status = StatusApolice.CANCELADA;
        }

        public void Expirar()
        {
            Status = StatusApolice.EXPIRADA;
        }

        public void Ativar()
        {
            Status = StatusApolice.ATIVA;
        }
    }
}
