using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Apolices.UpdateApoliceStatus
{
    public class UpdateApoliceStatusUseCase(IApoliceRepository apoliceRepository, IUnitOfWork unitOfWork) : IUpdateApoliceStatusUseCase
    {
        private readonly IApoliceRepository _apoliceRepository = apoliceRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task ExecuteAsync(int apoliceId, StatusApolice novoStatus)
        {
            var apolice = await _apoliceRepository.GetApoliceByIdAsyncTracked(apoliceId);
            if (apolice is null)
                throw new DomainException("Apolice não encontrada.");

            switch (novoStatus)
            {
                case StatusApolice.ATIVA:
                    apolice.Activate();
                    break;
                case StatusApolice.EXPIRADA:
                    apolice.Expire();
                    break;
                case StatusApolice.CANCELADA:
                    apolice.Cancel();
                    break;
                default:
                    throw new DomainException("Status inválido.");
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
