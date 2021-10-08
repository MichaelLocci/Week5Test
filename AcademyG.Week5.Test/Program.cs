using AcademyG.Week5.Lib;
using System;
using System.Collections.Generic;

namespace AcademyG.Week5.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Main loop

            bool quit = false;
            do
            {
                string command = ConsoleHelpers.BuildMenu("Main Menu",
                    new List<string> {
                        "[ 1 ] - Inserimento nuova Spesa",
                        "[ 2 ] - Approvazione Spese Esistenti",
                        "[ 3 ] - Cancellazione Spese",
                        "[ 4 ] - Modifica Spesa",
                        "[ 5 ] - Elenco Spese Approvate",
                        "[ 6 ] - Elenco Spese Utente",
                        "[ 7 ] - Elenco Spese per Categoria",
                        "[ 8 ] - Elenco Spese",
                        "[ 9 ] - Elenco Spese ADONET",
                        "[ q ] - QUIT"
                    });

                switch (command)
                {
                    case "1":
                        // agginta spesa
                        Client.AddSpesa();
                        break;
                    case "2":
                        // approvazione spese
                        Client.ApprovazioneSpesa();
                        break;
                    case "3":
                        // elimizaione spese
                        Client.DeleteSpesa();
                        break;
                    case "4":
                        // modifica spesa
                        Client.ModificaSpesa();
                        break;
                    case "5":
                        // elenco spese approvate
                        Client.ListaSpese(s => s.Approvato == true);
                        break;
                    case "6":
                        // elenco spese utente
                        string user = ConsoleHelpers.GetData("Inserisci il nome utente");
                        Client.ListaSpese(s => s.Utente == user);
                        break;
                    case "7":
                        // totale spesa per categoria
                        Client.TotaleSpesePerCategoria();
                        break;
                    case "8":
                        // elenco spese
                        Client.ListaSpese();
                        break;
                    case "9":
                        // elenco spese
                        Client.ListaSpesaADONET();
                        break;
                    case "q":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Comando sconosciuto.");
                        break;
                }

            } while (!quit);

            #endregion
        }
    }
}
