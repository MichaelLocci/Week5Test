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
    public class ConfigurazioneSpesa : IEntityTypeConfiguration<Spesa>
    {

        public void Configure(EntityTypeBuilder<Spesa> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Data)
                .IsRequired();
            builder.Property(s => s.Descrizione)
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(s => s.Utente)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(s => s.Importo)
                .HasPrecision(5, 2)
                .IsRequired();
            builder.Property(s => s.Approvato)
                .IsRequired();

            builder
                .HasOne(c => c.Categoria)
                .WithMany(s => s.Spese);
        }
    }
}
