using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
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
            if (input.Length < 3 || input.Length > 50)
            {
                return "Error: The string must be between 3 and 50 characters long.";
            }

            // Check if the string starts with a letter and contains only alphanumeric characters
            if (!Regex.IsMatch(input, @"^[a-zA-Z][a-z_A-Z0-9]*$"))
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

        public static void runsqlscript(string sqlFile, string mariadbexe, string dbUser, string dbPass, string dbHost, Logging log)
        {
            if (!File.Exists(sqlFile))
            {
                throw new Exception($"Error: File '{sqlFile}' does not exist.");
            }

            log.writeLine($"Executing SQL file: {sqlFile}");

            // Create a temporary batch file to handle the redirection
            string batchFilePath = Path.Combine(Path.GetTempPath(), $"mariadb_script_{Guid.NewGuid()}.cmd");

            // Create the batch file content
            string batchContent = $"\"{mariadbexe}\" -u{dbUser} -p{dbPass} -h{dbHost} < \"{sqlFile}\"";
            File.WriteAllText(batchFilePath, batchContent);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{batchFilePath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = false
            };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Clean up the temporary batch file
                try { File.Delete(batchFilePath); } catch { /* Ignore cleanup errors */ }

                if (process.ExitCode != 0)
                {
                    log.writeError($"Error executing file: {sqlFile}. Exit code: {process.ExitCode}");
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        log.writeError("Error details: " + error);
                    }
                }
                else
                {
                    log.writeLine($"Successfully executed file: {sqlFile}.");
                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        log.writeLine("Client output: " + output);
                    }
                }
            }
        }

        public static void rundbquery(string query, string server, string user, string password, string databaseName)
        {
            // Stringa di connessione al server MariaDB
            string connectionString = $"Server={server};Database={databaseName};User ID={user};Password={password};";

            using (var connection = new MySqlConnection(connectionString))
            {
                // Apri la connessione
                connection.Open();

                // Esegui la query
                using (var command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /*
        // Eseguire una query semplice
        var result1 = DatabaseService.ExecuteQuery(connectionString, "SELECT * FROM users WHERE age > @age",
            new List<MySqlParameter> { new MySqlParameter("@age", 18) });

        // Eseguire una query complessa
        var result2 = DatabaseService.ExecuteQuery(connectionString,"SELECT name, email, COUNT(*) AS total FROM orders GROUP BY name, email");
         */
        public static List<Dictionary<string, object>> runselectquery(string connectionString, string query, List<MySqlParameter> parameters = null)
        {
            var results = new List<Dictionary<string, object>>();

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }

                            results.Add(row);
                        }
                    }
                }
            }
            return results;
        }

        public static string[] GetAlldb(string server, string userId, string password)
        {
            string[] ignoredb = { "information_schema","mysql","performance_schema","sys","phpmyadmin"};
            List<string> databases = new List<string>();
            string query = "SHOW DATABASES";
            string connectionString = $"Server={server};User ID={userId};Password={password};";

            var results = runselectquery(connectionString, query);
            foreach (var row in results)
            {
                string name = row["Database"].ToString();
                if (!ignoredb.Contains(name))
                    databases.Add(name);
            }

            return databases.ToArray();
        }

        public static string[] GetAllWPFolder(string folderPath)
        {
            string[] arrdir = Directory.GetDirectories(folderPath)
                       .Where(dir => File.Exists(Path.Combine(dir, "wp-config.php")))
                       .Select(Path.GetFileName)
                       .ToArray();
            return arrdir;
        }

    }
}
