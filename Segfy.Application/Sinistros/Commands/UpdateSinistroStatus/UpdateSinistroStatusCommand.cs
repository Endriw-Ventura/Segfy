using MediatR;
using segfy.Domain.Enums;

namespace Segfy.Application.Sinistros.Commands.UpdateSinistroStatus
{
    public sealed record UpdateSinistroStatusCommand(
        int Id,
        StatusSinistro Status,
        decimal? ValorAprovado,
        string? MotivoNegativa
    ) : IRequest<Unit>;
}
