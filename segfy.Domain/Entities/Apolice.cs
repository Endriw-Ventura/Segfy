using segfy.Domain.Enums;
using Segfy.Domain.Enums;

namespace segfy.Domain.Entities
{
    public class Apolice
    {
        public int Id { get; set; }
        public string NumeroApolice { get; private set; } 
        public string NomeSegurado { get; private set; }
        public DateTime DataInicio { get; private set; } 
        public DateTime DataFim { get; private set; } 
        public StatusApolice Status { get; private set; } = StatusApolice.ATIVA;
        public Ramo Ramo { get; set; }

        public Apolice(string numeroApolice, string nomeSegurado, DateTime dataInicio, DateTime dataFim, Ramo ramo)
        {
            NumeroApolice = numeroApolice;
            NomeSegurado = nomeSegurado;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }
        protected Apolice() { }
        
        private readonly List<Sinistro> _sinistros = [];
        public IReadOnlyCollection<Sinistro> Sinistros => _sinistros.AsReadOnly();

        public bool IsAtiva()
        {
            return Status == StatusApolice.ATIVA;
        }

        public void Cancel()
        {
            Status = StatusApolice.CANCELADA;
        }

        public void Expire()
        {
            Status = StatusApolice.EXPIRADA;
        }

        public void Activate()
        {
            Status = StatusApolice.ATIVA;
        }
    }
}
