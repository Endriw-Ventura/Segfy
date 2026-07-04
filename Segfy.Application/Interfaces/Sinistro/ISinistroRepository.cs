using segfy.Domain.Entities;
using segfy.Domain.Enums;
using DomainSinistro = segfy.Domain.Entities.Sinistro;

namespace Segfy.Application.Interfaces.Sinistro
{
    public interface ISinistroRepository
    {
        Task<DomainSinistro?> GetSinistroByIdAsync(int id);
        Task<DomainSinistro?> GetSinistroByIdAsyncTracked(int id);

        Task<IEnumerable<DomainSinistro>> GetAllAsync(StatusSinistro? status, DateTime? data, int page, int pageSize);
        Task AddSinistroAsync(DomainSinistro sinistro);
        Task<IEnumerable<HistoricoSinistros>> GetHistoricoSinistro(int id);
    }
}
