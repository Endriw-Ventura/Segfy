using Segfy.Application.Interfaces;
using Segfy.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Infrastructure.Persistence
{
    public class UnitOfWork(AppDBContext context) : IUnitOfWork
    {
        private readonly AppDBContext _context = context;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
