using segfy.Domain.Enums;

namespace Segfy.API.Requests.Apolices
{
    public sealed record GetAllApolicesRequest(
        int? Page,
        int? PageSize,
        StatusSinistro? Status,
        DateTime? Data
        );
}
