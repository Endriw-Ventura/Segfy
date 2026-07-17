using segfy.Domain.Enums;

namespace Segfy.API.Requests.Sinistros
{
    public sealed record CreateSinistroRequest(
        string NumeroSinistro,
        DateTime DataSinistro,
        string Descricao,
        decimal ValorSolicitado,
        int ApoliceId
    );
}
