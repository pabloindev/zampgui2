using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZampGUI2_Console
{
    class RunBackupDB
    {
        string[] args { get; set; }
        public RunBackupDB(string[] args)
        {
            args = args;
        }

        public void run()
        {
            // Parametri per la connessione al server MariaDB.
            // Modifica questi valori in base alla tua configurazione.
            string dbUser = "root";
            string dbPass = "root";
            string dbHost = "localhost";

            string[] nuovoArray = array.Skip(1).ToArray();
            string combinedPaths = string.Join(" ", args.Select(p => $"\"{p}\""));

            // Creazione della stringa di connessione (senza specificare il database)
            string connectionString = $"server={dbHost};user={dbUser};password={dbPass};";

            // Definizione della cartella principale di backup, ad esempio "DatabaseBackups"
            string backupRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseBackups");
            // Nome della sottocartella corrispondente alla data corrente (formato "yyyyMMdd")
            string currentDateFolder = DateTime.Now.ToString("yyyyMMdd");
            string backupDir = Path.Combine(backupRoot, currentDateFolder);
            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            // Elenco dei database di sistema da escludere dal backup
            var excludedDatabases = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "information_schema",
                "mysql",
                "performance_schema",
                "sys",
                "phpmyadmin"
            };

            // Recupero della lista di database tramite la query SHOW DATABASES
            List<string> databases = new List<string>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SHOW DATABASES;", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dbName = reader.GetString(0);
                            if (!excludedDatabases.Contains(dbName))
                            {
                                databases.Add(dbName);
                            }
                        }
                    }
                }
            }

            // Per ogni database valido viene eseguito il backup tramite mysqldump.
            foreach (string db in databases)
            {
                // Creazione del suffisso con l'ora corrente compresa di millisecondi (formato "HHmmssfff")
                string timestamp = DateTime.Now.ToString("HHmmssfff");
                // Nome del file SQL: [nomeDatabase]_[timestamp].sql
                string fileName = $"{db}_{timestamp}.sql";
                string filePath = Path.Combine(backupDir, fileName);

                // Costruisco la stringa degli argomenti per mysqldump:
                // --add-drop-database aggiunge le istruzioni DROP/CREATE per il database;
                // --databases include il comando USE e le informazioni per la creazione del database;
                // --routines e --triggers permettono di esportare stored procedure, funzioni e trigger.
                // L'opzione --result-file permette di salvare direttamente il dump nel file specificato.
                string dumpArguments = $"-u{dbUser} -p{dbPass} -h{dbHost} --add-drop-database --databases {db} --routines --triggers --result-file=\"{filePath}\"";

                Console.WriteLine($"Avvio backup del database: {db}");
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "mysqldump",
                    Arguments = dumpArguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                    // Non è necessario redirigere lo standard output se usiamo --result-file.
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        string errorOutput = process.StandardError.ReadToEnd();
                        Console.Error.WriteLine($"Errore durante il backup del database {db}: {errorOutput}");
                    }
                    else
                    {
                        Console.WriteLine($"Backup completato per il database {db}.\nFile salvato in: {filePath}");
                    }
                }
            }
        }


        

        public bool dbExist()
        {
            // Stringa di connessione al server MariaDB
            string connectionString = "Server=localhost;User ID=root;Password=your_password;";

            // Nome del database da verificare
            string databaseName = "nome_database";

            // Query per verificare se il database esiste
            string query = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}';";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // Apri la connessione
                    connection.Open();

                    // Esegui la query
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine($"Il database '{databaseName}' esiste già.");
                        }
                        else
                        {
                            Console.WriteLine($"Il database '{databaseName}' non esiste.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore: {ex.Message}");
                }
            }
        }

    }
}
