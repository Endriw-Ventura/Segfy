using Microsoft.EntityFrameworkCore;
using segfy.Domain.Entities;
using segfy.Domain.Enums;
using Segfy.Application.Interfaces.Apolices;
using Segfy.Infrastructure.Persistence.Context;

namespace Segfy.Infrastructure.Persistence.Repositories
{
    public class ApoliceRepository(AppDBContext context) : IApoliceRepository
    {
        private readonly AppDBContext _context = context;

        public async Task AddApoliceAsync(Apolice apolice, CancellationToken cancellationToken)
        {
            await _context.Apolices.AddAsync(apolice, cancellationToken);
        }

        public async Task<bool> CheckForDuplicateNumeroApolice(string numeroApolice, CancellationToken cancellationToken)
        {
            return await _context.Apolices.AnyAsync(a => a.NumeroApolice == numeroApolice, cancellationToken);
        }

        public async Task DeleteApoliceAsync(int id, CancellationToken cancellationToken)
        {
            await _context.Apolices.Where(a => a.Id == id).ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<IEnumerable<Apolice>> GetAllAsync(StatusApolice? status, DateTime? data, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Apolices
                 .AsNoTracking()
                 .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            if (data.HasValue)
            {
                query = query.Where(x => x.DataInicio.Date == data.Value.Date);
            }

            query = query.OrderByDescending(x => x.DataInicio);

            return await query
                .Skip(((page - 1) * pageSize))
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Apolice?> GetApoliceByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Apolices.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Apolice?> GetApoliceByIdAsyncTracked(int id, CancellationToken cancellationToken)
        {
            return await _context.Apolices.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Apolice?> GetByIdWithSinistrosAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Apolices
           .Include(x => x.Sinistros)
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
