using Segfy.Application.DTOs.HistoricoSinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.GetHistoricoSinistro
{
    public interface IGetHistoricoSinistroUseCase
    {
        Task<IEnumerable<HistoricoSinistroDTO>> ExecuteAsync(int id);
    }
}
