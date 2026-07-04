using Microsoft.EntityFrameworkCore;
using segfy.Domain.Entities;

namespace Segfy.Infrastructure.Configurations
{
    public class HistoricoSinistrosConfiguration : IEntityTypeConfiguration<HistoricoSinistros>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<HistoricoSinistros> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Sinistro)
                .WithMany(x => x.Historicos)
                .HasForeignKey(x => x.SinistroId);
        }
    }
}
