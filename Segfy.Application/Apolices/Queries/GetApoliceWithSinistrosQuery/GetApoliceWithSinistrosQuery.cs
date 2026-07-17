using MediatR;
using Segfy.Application.Apolices.Results;

namespace Segfy.Application.Apolices.Queries.GetApoliceWithSinistrosQuery
{
    public sealed record GetApoliceWithSinistrosQuery(int Id): IRequest<GetApoliceWithSinistrosQueryResult>;
}
