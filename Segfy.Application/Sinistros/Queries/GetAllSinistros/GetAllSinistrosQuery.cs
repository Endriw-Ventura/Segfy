using MediatR;
using segfy.Domain.Enums;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Sinistros.Queries.GetAllSinistros
{
    public sealed record GetAllSinistrosQuery(
        StatusSinistro? StatusSinistro,
        DateTime? Data,
        int Page = 0,
        int PageSize = 10
    ) : IRequest<IEnumerable<SinistroResult>>;
}
