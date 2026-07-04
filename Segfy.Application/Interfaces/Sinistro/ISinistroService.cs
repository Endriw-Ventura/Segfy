using segfy.Domain.Entities;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.HistoricoSinistro;
using Segfy.Application.DTOs.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.Interfaces.Sinistro
{
    public interface ISinistroService
    {
        Task<SinistroDTO> AbrirSinistroAsync(CreateSinistroDTO request);
        Task AtualizarStatusAsync(
       int sinistroId,
       StatusSinistro novoStatus,
       string? motivoNegativa,
       decimal? valorAprovado);
        Task<IEnumerable<SinistroDTO>> GetAllAsync(StatusSinistro? status, DateTime? data, int page, int pageSize);
        Task<IEnumerable<HistoricoSinistroDTO>> GetHistoricoSinistro(int id);
        Task<SinistroDTO?> GetSinistroByIdAsync(int id);
    }
}
