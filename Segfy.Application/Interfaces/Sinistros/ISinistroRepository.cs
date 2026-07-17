using segfy.Domain.Entities;
using segfy.Domain.Enums;
namespace Segfy.Application.Interfaces.Sinistros
{
    public interface ISinistroRepository
    {
        Task<Sinistro?> GetSinistroByIdAsync(int id, CancellationToken cancellationToken);
        Task<Sinistro?> GetSinistroByIdAsyncTracked(int id, CancellationToken cancellationToken);

        Task<IEnumerable<Sinistro>> GetAllAsync(StatusSinistro? status, DateTime? data, int page, int pageSize, CancellationToken cancellationToken);
        Task AddSinistroAsync(Sinistro sinistro, CancellationToken cancellationToken);
        Task<IEnumerable<HistoricoSinistros>> GetHistoricoSinistro(int id, CancellationToken cancellationToken);
        Task<bool> CheckForDuplicateNumeroSinistroAsync(string numeroSinistro, CancellationToken cancellationToken);
    }
}
