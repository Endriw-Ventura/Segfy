using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace segfy.Domain.Entities
{
    public class Sinistro
    {
        public int Id { get; set; }
        public string NumeroSinistro { get; private set; }
        public DateTime DataSinistro { get; private set; }
        public string Descricao { get; private set; }
        public decimal ValorSolicitado { get; private set; }
        public decimal? ValorAprovado { get; private set; }
        public string? MotivoNegativa { get; private set; }
        public StatusSinistro Status { get; private set; }
        public int ApoliceId { get; private set; }
        public Apolice Apolice { get; private set; }

        private readonly List<HistoricoSinistros> _historicos = [];
        public IReadOnlyCollection<HistoricoSinistros> Historicos => _historicos.AsReadOnly();


        public Sinistro(string numeroSinistro, DateTime dataSinistro, string descricao, decimal? valor, Apolice apolice)
        {
            if (apolice is null)
                throw new DomainException("A apólice é obrigatória.");

            if (!apolice.IsAtiva())
                throw new DomainException("Sinistro só pode ser aberto em apólice ativa.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new DomainException("A descrição do sinistro é obrigatória.");

            if (valor <= 0)
                throw new DomainException("O valor solicitado deve ser maior que zero.");

            NumeroSinistro = numeroSinistro;
            DataSinistro = dataSinistro;
            Descricao = descricao;
            ValorAprovado = valor;
            ApoliceId = apolice.Id;
            Apolice = apolice;
            Status = StatusSinistro.ABERTO;
        }

        public bool PossivelAtualizarStatus(StatusSinistro novoStatus)
        {
            return Status switch
            {
                StatusSinistro.ABERTO => novoStatus == StatusSinistro.EM_ANALISE,
                StatusSinistro.EM_ANALISE => novoStatus == StatusSinistro.APROVADO || novoStatus == StatusSinistro.NEGADO,
                StatusSinistro.APROVADO => novoStatus == StatusSinistro.ENCERRADO,
                StatusSinistro.ENCERRADO => false,
                StatusSinistro.NEGADO => false,
                _ => false
            };
        }

        private void AdicionarHistorico(
        StatusSinistro? statusAnterior,
        StatusSinistro statusNovo,
        string? observacao)
        {
            _historicos.Add(new HistoricoSinistros(
                Id,
                statusAnterior,
                statusNovo,
                observacao));
        }

        public void EnviarParaAnalise()
        {
            AlterarStatus(StatusSinistro.EM_ANALISE);
        }

        public void AlterarStatus(StatusSinistro novoStatus)
        {
            if (!PossivelAtualizarStatus(novoStatus))
                throw new DomainException($"Não é permitido alterar o status de {Status} para {novoStatus}.");

            var statusAnterior = Status;
            Status = novoStatus;

            AdicionarHistorico(statusAnterior, novoStatus, null);
        }

        public void Aprovar()
        {
            AlterarStatus(StatusSinistro.APROVADO);
        }

        public void Encerrar(decimal? valorAprovado)
        {
            if (valorAprovado <= 0)
                throw new DomainException("O valor aprovado é obrigatório para encerrar o sinistro.");

            ValorAprovado = valorAprovado;

            AlterarStatus(StatusSinistro.ENCERRADO);
        }

        public void Negar(string? motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new DomainException("O motivo da negativa é obrigatório para negativar o sinistro.");

            MotivoNegativa = motivo;

            AlterarStatus(StatusSinistro.NEGADO);
        }

    }
}
