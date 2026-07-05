using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.UpdateSinistroStatus
{
    public class UpdateSinistroStatusUseCase(ISinistroRepository sinistroRepository, IUnitOfWork unitOfWork) : IUpdateSinistroStatusUseCase
    {
        private readonly ISinistroRepository _sinistroRepository = sinistroRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task ExecuteAsync(int sinistroId, StatusSinistro novoStatus, string? motivoNegativa, decimal? valorAprovado)
        {
            var sinistro = await _sinistroRepository.GetSinistroByIdAsyncTracked(sinistroId);

            if (sinistro is null)
                throw new DomainException("Sinistro não encontrado.");

            switch (novoStatus)
            {
                case StatusSinistro.EM_ANALISE:
                    sinistro.SendForAnalisys();
                    break;
                case StatusSinistro.APROVADO:
                    sinistro.Aprove();
                    break;
                case StatusSinistro.ENCERRADO:
                    sinistro.Close(valorAprovado);
                    break;
                case StatusSinistro.NEGADO:
                    sinistro.Deny(motivoNegativa);
                    break;
                default:
                    throw new DomainException("Status inválido.");
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
