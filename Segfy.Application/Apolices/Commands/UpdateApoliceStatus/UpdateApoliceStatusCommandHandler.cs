using MediatR;
using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolices;

namespace Segfy.Application.Apolices.Commands.UpdateApoliceStatus
{
    public class UpdateApoliceStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IApoliceRepository apoliceRepository) : IRequestHandler<UpdateApoliceStatusCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateApoliceStatusCommand request, CancellationToken cancellationToken)
        {
            var apolice = await apoliceRepository.GetApoliceByIdAsyncTracked(request.Id, cancellationToken);
            if (apolice is null)
                throw new DomainException("Apolice não encontrada.");

            switch (request.Status)
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
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
