
using segfy.Domain.Enums;

namespace Segfy.Application.Sinistros.Results
{
    public sealed record CreateSinistroResult(
        int Id,
        string NumeroSinistro,
        DateTime DataSinistro, 
        string Descricao,
        decimal ValorSolicitado,
        decimal? ValorAprovado,
        string? MotivoNegativa,
        StatusSinistro Status 
    );
}
