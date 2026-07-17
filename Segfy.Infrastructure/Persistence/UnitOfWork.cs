using Segfy.Application.Interfaces;
using Segfy.Infrastructure.Persistence.Context;

namespace Segfy.Infrastructure.Persistence
{
    public class UnitOfWork(AppDBContext context) : IUnitOfWork
    {
        private readonly AppDBContext _context = context;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
