using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using segfy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Infrastructure.Configurations
{
    public class ApoliceConfiguration : IEntityTypeConfiguration<Apolice>
    {
        public void Configure(EntityTypeBuilder<Apolice> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.NumeroApolice)
                .IsUnique();

            builder.Property(x => x.NumeroApolice)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
