using MediatR;
using Segfy.Application.Apolices.Results;

namespace Segfy.Application.Apolices.Queries.GetApoliceByIdQuery
{
    public sealed record class GetApoliceByIdQuery(int Id) : IRequest<ApoliceResult>;
}
