using segfy.Domain.Enums;

namespace Segfy.API.Requests.Apolices
{
    public sealed record UpdateApoliceStatusRequest(
        StatusApolice Status
        );
}
