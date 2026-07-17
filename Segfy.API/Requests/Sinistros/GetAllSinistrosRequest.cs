using MediatR;
using Microsoft.AspNetCore.Mvc;
using segfy.Domain.Enums;

namespace Segfy.API.Requests.Sinistros
{
    public sealed record GetAllSinistrosRequest(
        StatusSinistro? Status,
        DateTime? Data,
        int Page = 0,
        int PageSize = 10
    );
}
