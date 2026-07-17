using MediatR;
using Segfy.Application.Apolices.Results;
using Segfy.Domain.Enums;

namespace Segfy.Application.Apolices.Commands.CreateApolice
{
    public sealed record CreateApoliceCommand(
        string NumeroApolice,
        string NomeSegurado,
        DateTime DataInicio,
        DateTime DataFim,
        Ramo Ramo
    ) : IRequest<CreateApoliceResult>;
}
