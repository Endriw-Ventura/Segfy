using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using segfy.Domain.Entities;

namespace Segfy.Infrastructure.Configurations
{
    public class SinistroConfiguration : IEntityTypeConfiguration<Sinistro>
    {
        public void Configure(EntityTypeBuilder<Sinistro> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.NumeroSinistro)
                .IsUnique();
            
            builder.Property(x => x.NumeroSinistro)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ValorSolicitado)
                .HasPrecision(18, 2);

            builder.HasOne(x => x.Apolice)
                .WithMany(x => x.Sinistros)
                .HasForeignKey(x => x.ApoliceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
