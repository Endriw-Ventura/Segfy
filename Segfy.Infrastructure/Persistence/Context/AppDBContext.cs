using Microsoft.EntityFrameworkCore;
using segfy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Infrastructure.Persistence.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){}
        public DbSet<Apolice> Apolices => Set<Apolice>();
        public DbSet<Sinistro> Sinistros => Set<Sinistro>();
        public DbSet<HistoricoSinistros> HistoricoSinistros => Set<HistoricoSinistros>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
