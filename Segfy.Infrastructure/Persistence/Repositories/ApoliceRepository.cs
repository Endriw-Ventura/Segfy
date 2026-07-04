using Microsoft.EntityFrameworkCore;
using segfy.Domain.Entities;
using segfy.Domain.Enums;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Infrastructure.Persistence.Context;

namespace Segfy.Infrastructure.Persistence.Repositories
{
    public class ApoliceRepository(AppDBContext context) : IApoliceRepository
    {
        private readonly AppDBContext _context = context;

        public async Task AddApoliceAsync(Apolice apolice)
        {
            await _context.Apolices.AddAsync(apolice);
        }

        public async Task<bool> CheckForDuplicateNumeroApolice(string numeroApolice)
        {
            return await _context.Apolices.AnyAsync(a => a.NumeroApolice == numeroApolice);
        }

        public async Task DeleteApoliceAsync(int id)
        {
            await _context.Apolices.Where(a => a.Id == id).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Apolice>> GetAllAsync(StatusApolice? status, DateTime? data, int page, int pageSize)
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
                .ToListAsync();
        }

        public async Task<Apolice?> GetApoliceByIdAsync(int id)
        {
            return await _context.Apolices.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Apolice?> GetApoliceByIdAsyncTracked(int id)
        {
            return await _context.Apolices.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Apolice?> GetByIdWithSinistrosAsync(int id)
        {
            return await _context.Apolices
           .Include(x => x.Sinistros)
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task UpdateApoliceAsync(Apolice newApolice)
        {
            _context.Apolices.Update(newApolice);
            return Task.CompletedTask;
        }


    }
}
