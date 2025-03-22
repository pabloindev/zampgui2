using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZampGUI2_Console
{
    public class Helper
    {
        public static string nameOK(string input, bool allowNullOrEmpty = false)
        {
            // Check for null or empty string if not allowed
            if (!allowNullOrEmpty && string.IsNullOrEmpty(input))
            {
                return "Error: The string cannot be null or empty.";
            }

            // If null or empty is allowed and the input is null or empty, skip further checks
            if (allowNullOrEmpty && string.IsNullOrEmpty(input))
            {
                return "";
            }

            // Check length constraints
            if (input.Length < 3 || input.Length > 25)
            {
                return "Error: The string must be between 3 and 25 characters long.";
            }

            // Check if the string starts with a letter and contains only alphanumeric characters
            if (!Regex.IsMatch(input, @"^[a-zA-Z][a-zA-Z0-9]*$"))
            {
                return "Error: The string must start with a letter and contain only alphanumeric characters.";
            }

            // If all checks pass
            return "";
        }

        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return "Error: Email cannot be null or empty.";

            // Espressione regolare per validare un'email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(email, pattern))
                return "Error: Invalid email format.";

            return string.Empty; // Nessun errore, email valida
        }

        public static bool dbExist(string server, string user, string password, string databaseName)
        {
            // Stringa di connessione al server MariaDB
            string connectionString = $"Server={server};User ID={user};Password={password};";

            // Query per verificare se il database esiste
            string query = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}';";

            using (var connection = new MySqlConnection(connectionString))
            {
                // Apri la connessione
                connection.Open();

                // Esegui la query
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        //Console.WriteLine($"Il database '{databaseName}' esiste già.");
                        return true;
                    }
                    else
                    {
                        //Console.WriteLine($"Il database '{databaseName}' non esiste.");
                        return false;
                    }
                }
            }
        }
    }
}
