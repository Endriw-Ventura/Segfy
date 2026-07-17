using MediatR;
using Segfy.Application.Sinistros.Results;

namespace Segfy.Application.Sinistros.Commands.CreateSinistro
{
    public sealed record CreateSinistroCommand(
        string NumeroSinistro,
        DateTime DataSinistro,
        string Descricao,
        decimal ValorSolicitado,
        int ApoliceId
    ) :IRequest<CreateSinistroResult>;
}
