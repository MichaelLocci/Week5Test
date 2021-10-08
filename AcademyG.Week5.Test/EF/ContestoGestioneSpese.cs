using AcademyG.Week5.Test.EF.Configuration;
using AcademyG.Week5.Test.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyG.Week5.Test.EF
{
    public class ContestoGestioneSpese : DbContext
    {

        #region PROPRIETA

        public DbSet<Spesa> Spese { get; set; }
        public DbSet<Categoria> Categorie { get; set; }

        #endregion

        #region COSTRUTTORI

        public ContestoGestioneSpese() : base() { }

        public ContestoGestioneSpese(DbContextOptions<ContestoGestioneSpese> options) : base() { }

        #endregion

        #region OVERRIDE METODI DI CONFIGURAZIONE

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

                string connectionStringSQL = config.GetConnectionString("AcademyG");

                //usa sqlserver con questo indirizzo
                optionsBuilder.UseSqlServer(connectionStringSQL);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigurazioneSpesa());

            modelBuilder.ApplyConfiguration(new ConfigurazioneCategoria());
        }
        #endregion
    }
}
