using AcademyG.Week5.Test.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyG.Week5.Test.EF.Configuration
{
    class ConfigurazioneCategoria : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Descrizione)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .HasMany(s => s.Spese)
                .WithOne(c => c.Categoria);
        }
    }
}
