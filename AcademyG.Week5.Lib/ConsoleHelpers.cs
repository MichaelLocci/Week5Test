using System;
using System.Collections.Generic;

namespace AcademyG.Week5.Lib
{
    public static class ConsoleHelpers
    {
        #region Build Lists

        public static string BuildMenu(string title, List<string> menuItems)
        {
            Console.Clear();
            Console.WriteLine($"============= {title} =============");
            Console.WriteLine();

            foreach (var item in menuItems)
            {
                Console.WriteLine(item);
                Console.WriteLine();
            }

            // get command
            Console.Write("> ");
            string command = Console.ReadLine();
            Console.WriteLine();

            return command;
        }

        #endregion

        #region GetData

        public static string GetData(string message)
        {
            Console.Write(message + ": ");
            var value = Console.ReadLine();
            return value;
        }

        public static string GetData(string message, string initialValue)
        {
            Console.Write(message + $" ({initialValue}): ");
            var value = Console.ReadLine();
            return string.IsNullOrEmpty(value) ? initialValue : value;
        }

        #endregion

        #region GetEnum

        public static T GetEnum<T>(string fieldName, T state = default(T)) where T : struct
        {
            string enumLegenda = $"{fieldName} [";
            foreach (var suit in Enum.GetValues(typeof(T)))
            {
                enumLegenda += suit.ToString() + "/";
            }
            enumLegenda += "] > ";

            Console.Write(enumLegenda);
            var value = Console.ReadLine();

            Enum.TryParse<T>(value, true, out T result);

            return result;
        }

        #endregion

    }
}
