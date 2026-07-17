using Segfy.Domain.Enums;

namespace Segfy.API.Requests.Apolices
{
    public sealed record CreateApoliceRequest(
        string NumeroApolice,
        string NomeSegurado,
        DateTime DataInicio,
        DateTime DataFim,
        Ramo Ramo
        );
}
