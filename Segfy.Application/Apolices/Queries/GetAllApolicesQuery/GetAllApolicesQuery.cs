using MediatR;
using segfy.Domain.Enums;
using Segfy.Application.Apolices.Results;

namespace Segfy.Application.Apolices.Queries.GetAllApolicesQuery
{
    public sealed record class GetAllApolicesQuery(
        StatusApolice? StatusApolice,
        DateTime? Data,
        int Page = 0,
        int PageSize = 10
        ) : IRequest<IEnumerable<ApoliceResult>>;
}