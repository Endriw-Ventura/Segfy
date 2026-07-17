using MediatR;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Sinistros.Queries.GetSinistroById
{
    public sealed record GetSinistroByIdQuery(int Id) : IRequest<SinistroResult>;
}
