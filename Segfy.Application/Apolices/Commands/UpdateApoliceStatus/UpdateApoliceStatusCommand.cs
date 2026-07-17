using MediatR;
using segfy.Domain.Enums;

namespace Segfy.Application.Apolices.Commands.UpdateApoliceStatus
{
    public sealed record UpdateApoliceStatusCommand(
        int Id,
        StatusApolice Status
    ) : IRequest<Unit>;
}
