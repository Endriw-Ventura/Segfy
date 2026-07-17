using segfy.Domain.Enums;

namespace Segfy.API.Requests.Sinistros
{
    public sealed record UpdateSinistroStatusRequest(StatusSinistro Status, decimal? ValorAprovado, string? MotivoRecusa);
}
