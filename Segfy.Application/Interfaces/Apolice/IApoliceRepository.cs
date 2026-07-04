using segfy.Domain.Enums;
using DomainApolice = segfy.Domain.Entities.Apolice;
namespace Segfy.Application.Interfaces.Apolice
{
    public interface IApoliceRepository
    {
        Task<DomainApolice?> GetApoliceByIdAsync(int id);
        Task<DomainApolice?> GetByIdWithSinistrosAsync(int id);
        Task<IEnumerable<DomainApolice>> GetAllAsync(StatusApolice? status, DateTime? data, int page, int pageSize);
        Task AddApoliceAsync(DomainApolice apolice);
        Task UpdateApoliceAsync(DomainApolice apolice);
        Task DeleteApoliceAsync(int id);
        Task<DomainApolice?> GetApoliceByIdAsyncTracked(int id);
    }
}
