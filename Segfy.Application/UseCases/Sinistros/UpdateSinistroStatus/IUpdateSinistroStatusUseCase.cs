using segfy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.UpdateSinistroStatus
{
    public interface IUpdateSinistroStatusUseCase
    {
        Task ExecuteAsync(int sinistroId, StatusSinistro novoStatus, string? motivoNegativa, decimal? valorAprovado);
    }
}
