using AcademyG.Week5.Lib;
using AcademyG.Week5.Test.EF;
using AcademyG.Week5.Test.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyG.Week5.Test
{
    public static class Client
    {
        static IConfigurationRoot config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
        static string connectionStringSQL = config.GetConnectionString("AcademyG");

        #region METODO DI AGGIUNTA SPESA

        public static void AddSpesa()
        {
            Console.Clear();
            Console.WriteLine("---- Inserimento Spesa ----");

            string descrizione = ConsoleHelpers.GetData("Descrizione");
            string utente = ConsoleHelpers.GetData("Utente");
            string importo = ConsoleHelpers.GetData("Inserisci l'importo della spesa");
            Decimal.TryParse(importo, out decimal importodec);

            string categoria = ConsoleHelpers.GetData("inserisci la categoria");

            using ContestoGestioneSpese ctx = new();

            var selectCategoria = ctx.Categorie.FirstOrDefault(
                p => p.Descrizione.ToUpper() == categoria.ToUpper()
            );

            Spesa spesa = new()
            {
                Data = DateTime.Now,
                Categoria = selectCategoria,
                Descrizione = descrizione,
                Utente = utente,
                Importo = importodec,
                Approvato = false
            };

            ctx.Spese.Add(spesa);
            ctx.SaveChanges();

            Console.WriteLine("Spesa aggiunta con successo");

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

        #endregion

        #region METODO APPROVAZIONE SPESE

        public static void ApprovazioneSpesa()
        {

            Console.Clear();
            Console.WriteLine("---- Approvazione Spese ----");

            ListaSpese(prompt: false);
            int IdApprovazione = int.Parse(ConsoleHelpers.GetData("Inserisci Id della Spesa"));

            using ContestoGestioneSpese ctx = new();
            var IdCheck = ctx.Spese.Find(IdApprovazione);
            if(IdCheck != null)
            {
                ctx.Spese.Find(IdApprovazione).Approvato = true;
                ctx.SaveChanges();
            }

            Console.WriteLine("Approvazione spesa avvenuta con successo");

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

        #endregion

        #region METODO ELIMINAZIONE SPESA

        public static void DeleteSpesa()
        {
            using ContestoGestioneSpese ctx = new();

            Console.Clear();
            Console.WriteLine("---- Approvazione Spese ----");

            ListaSpese(prompt: false);
            int IdDelete = int.Parse(ConsoleHelpers.GetData("Inserisci Id della Spesa"));
            var IdCheck = ctx.Spese.Find(IdDelete);
            if (IdCheck != null)
            {
                ctx.Spese.Remove(IdCheck);
                ctx.SaveChanges();
            }

            Console.WriteLine("Eliminazione spesa avvenuta con successo");

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

        #endregion

        #region METODI DI LETTURA

        public static void ListaSpese(Func<Spesa,bool> filter = null, bool prompt = true)
        {
            using ContestoGestioneSpese ctx = new();

            IEnumerable<Spesa> listaSpesa;
            if(filter != null)
            {
                listaSpesa = ctx.Spese
                    .Include(c => c.Categoria)
                    .Where(filter);
            }
            else
            {
                listaSpesa = ctx.Spese.Include(c => c.Categoria);
            }

            Console.Clear();
            Console.WriteLine("---- Elenco Tickets ----");
            Console.WriteLine();
            Console.WriteLine("{0,-5}{1,-40}{2,15}{3,10}{4,10}{5,15}{6,15}", "ID", "Descrizione", "Categoria", "Utente", "Importo", "Approvato", "Data");
            Console.WriteLine(new String('-', 110));

            foreach (Spesa s in listaSpesa)
            {
                string formattedDate = s.Data.ToString("dd-MMM-yyyy");
                Console.WriteLine("{0,-5}{1,-40}{2,15}{3,10}{4,10}{5,15}{6,15}",
                    s.Id, s.Descrizione, s.Categoria.Descrizione, s.Utente, s.Importo, s.Approvato, formattedDate);
            }
            Console.WriteLine(new String('-', 110));

            if (prompt)
            {
                Console.WriteLine("---- Premi un tasto ----");
                Console.ReadKey();
            }
            else
            {
                return;
            }

        }

        public static void TotaleSpesePerCategoria()
        {
            using ContestoGestioneSpese ctx = new();

            Console.Clear();
            Console.WriteLine("---- Numero Spese per Categoria ----");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("{0,-15} {1, 20}", "Categoria", "numero Spese");
            Console.WriteLine("---------------------------------");

            foreach (var item in ctx.Categorie.Include(s => s.Spese))
            {
                Console.WriteLine("{0,-15}{1,20}",
                    item.Descrizione, item.Spese.Count());
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine();

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

        public static void ListaSpesaADONET(bool prompt = true)
        {
            //gestisce per noi la connessione al database, attraverso i parametri passati
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();

                string sqlStatement = "SELECT * FROM Spese";

                SqlCommand readCommand = new SqlCommand();
                readCommand.Connection = conn;
                readCommand.CommandType = System.Data.CommandType.Text;
                readCommand.CommandText = sqlStatement;

                SqlDataReader reader = readCommand.ExecuteReader();

                Console.Clear();
                Console.WriteLine("---- Elenco Spese ----");
                Console.WriteLine();
                Console.WriteLine("{0,-5}{1,-40}{2,15}{3,10}{4,10}{5,15}{6,15}", "ID", "Descrizione", "CategoriaId", "Utente", "Importo", "Approvato", "Data");
                Console.WriteLine(new String('-', 110));
                while (reader.Read())
                {
                    string formattedDate = ((DateTime)reader["Data"]).ToString("dd-MMM-yyyy");
                    Console.WriteLine("{0,-5}{1,-40}{2,15}{3,10}{4,10}{5,15}{6,15}",
                        $"[{reader["Id"]}]",$"{reader["Descrizione"]}",$"{reader["CategoriaId"]}",$"{reader["Utente"]}",$"{reader["Importo"]}",$"{reader["Approvato"]}",$" {formattedDate}");
                }
                Console.WriteLine(new string('-', 110));

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
                if (prompt)
                {
                    Console.WriteLine("---- Premi un tasto ----");
                    Console.ReadKey();
                }
            }
        }

        #endregion

        #region METODO DI MODIFICA SPESA

        public static void ModificaSpesa()
        {
            using ContestoGestioneSpese ctx = new();

            ListaSpese(prompt: false);
            int IdModifica = int.Parse(ConsoleHelpers.GetData("Inserisci Id della Spesa da modificare"));
            var IdCheck = ctx.Spese.Find(IdModifica);
            if (IdCheck != null) {

                string command = ConsoleHelpers.GetData("inserisci il tipo di modifica:\n" +
                                                                                        "1 - Categoria\n" +
                                                                                        "2 - Descrizione\n" +
                                                                                        "3 - Utente\n" +
                                                                                        "4 - Importo\n");

                switch (command)
                {
                    case "1":
                        string categoria = ConsoleHelpers.GetData("inserisci la nuova categoria");

                        var selectCategoria = ctx.Categorie.FirstOrDefault(
                            p => p.Descrizione.ToUpper() == categoria.ToUpper()
                        );
                        if (selectCategoria != null)
                        {
                            ctx.Spese.Find(IdModifica).Categoria = selectCategoria;
                            ctx.SaveChanges();
                        }
                        break;
                    case "2":
                        string descr = ConsoleHelpers.GetData("inserisci la nuova descrizione");
                        if (descr != null)
                        {
                            ctx.Spese.Find(IdModifica).Descrizione = descr;
                            ctx.SaveChanges();
                        }
                        break;
                    case "3":
                        string user = ConsoleHelpers.GetData("inserisci il nuovo utente");
                        if (user != null)
                        {
                            ctx.Spese.Find(IdModifica).Utente = user;
                            ctx.SaveChanges();
                        }
                        break;
                    case "4":
                        string importo = ConsoleHelpers.GetData("Inserisci il nuovo importo");
                        Decimal.TryParse(importo, out decimal importodec);
                        ctx.Spese.Find(IdModifica).Importo = importodec;
                        ctx.SaveChanges();
                        break;
                    default:
                        Console.WriteLine("Comando sconosciuto.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Id non valido");
            }
        }
        #endregion

    }
}
