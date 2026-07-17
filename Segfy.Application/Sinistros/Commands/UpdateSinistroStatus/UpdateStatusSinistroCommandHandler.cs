using AutoMapper;
using MediatR;
using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Sinistros;

namespace Segfy.Application.Sinistros.Commands.UpdateSinistroStatus
{
    public sealed class UpdateStatusSinistroCommandHandler(
        ISinistroRepository sinistroRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<UpdateSinistroStatusCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateSinistroStatusCommand command, CancellationToken cancellationToken)
        {
            var sinistro = await sinistroRepository.GetSinistroByIdAsync(command.Id, cancellationToken);
            
            if (sinistro is null)
                throw new DomainException("Sinistro não encontrado.");

            switch (command.Status)
            {
                case StatusSinistro.EM_ANALISE:
                    sinistro.SendForAnalisys();
                    break;
                case StatusSinistro.APROVADO:
                    sinistro.Aprove();
                    break;
                case StatusSinistro.ENCERRADO:
                    sinistro.Close(command.ValorAprovado);
                    break;
                case StatusSinistro.NEGADO:
                    sinistro.Deny(command.MotivoNegativa);
                    break;
                default:
                    throw new DomainException("Status inválido.");
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
