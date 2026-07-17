using MediatR;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Sinistros.Queries.GetHistoricoSinistro
{
    public sealed record GetHistoricoSinistroQuery(int Id) : IRequest<IEnumerable<GetHistoricoSinistroResult>>;
}
