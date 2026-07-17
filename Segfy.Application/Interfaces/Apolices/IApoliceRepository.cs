using segfy.Domain.Entities;
using segfy.Domain.Enums;
namespace Segfy.Application.Interfaces.Apolices
{
    public interface IApoliceRepository
    {
        Task<Apolice?> GetApoliceByIdAsync(int id, CancellationToken cancellationToken);
        Task<Apolice?> GetByIdWithSinistrosAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Apolice>> GetAllAsync(StatusApolice? status, DateTime? data, int page, int pageSize, CancellationToken cancellationToken);
        Task AddApoliceAsync(Apolice apolice, CancellationToken cancellationToken);
        Task DeleteApoliceAsync(int id, CancellationToken cancellationToken);
        Task<Apolice?> GetApoliceByIdAsyncTracked(int id, CancellationToken cancellationToken);
        Task<bool> CheckForDuplicateNumeroApolice(string numeroApolice, CancellationToken cancellationToken);
    }
}
