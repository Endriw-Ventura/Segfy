using Microsoft.EntityFrameworkCore;
using segfy.Domain.Entities;
using segfy.Domain.Enums;
using Segfy.Application.Interfaces.Sinistros;
using Segfy.Infrastructure.Persistence.Context;

namespace Segfy.Infrastructure.Persistence.Repositories
{
    public class SinistroRepository(AppDBContext context) : ISinistroRepository
    {
        private readonly AppDBContext _context = context;
        public async Task AddSinistroAsync(Sinistro sinistro, CancellationToken cancellationToken)
        {
            await _context.Sinistros.AddAsync(sinistro, cancellationToken);
        }

        public async Task<IEnumerable<Sinistro>> GetAllAsync(StatusSinistro? status, DateTime? data, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Sinistros
                .AsNoTracking()
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            if (data.HasValue)
            {
                query = query.Where(x => x.DataSinistro.Date == data.Value.Date);
            }

            query = query.OrderByDescending(x => x.DataSinistro);

            return await query
                .Skip(((page - 1) * pageSize))
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Sinistro?> GetSinistroByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Sinistros.Include(s => s.Apolice).AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<Sinistro?> GetSinistroByIdAsyncTracked(int id, CancellationToken cancellationToken)
        {
            return await _context.Sinistros
                .Include(s => s.Historicos)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<HistoricoSinistros>> GetHistoricoSinistro(int id, CancellationToken cancellationToken)
        {
            return await _context.HistoricoSinistros
                .AsNoTracking()
                .Where(h => h.SinistroId == id)
                .OrderByDescending(h => h.CriadoEm)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> CheckForDuplicateNumeroSinistroAsync(string numeroSinistro, CancellationToken cancellationToken)
        {
            return await _context.Sinistros.AnyAsync(a => a.NumeroSinistro == numeroSinistro, cancellationToken);
        }
    }
}
