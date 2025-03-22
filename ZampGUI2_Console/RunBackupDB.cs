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
        Logging log { get; set; }
        IniFile ini { get; set; }
        string ZAMPGUIPATH { get; set; }
        string UUID { get; set; }
        string CURRENT_VERS { get; set; }
        string MARIADBBIN { get; set; }
        public RunBackupDB(string[] args, Logging log)
        {
            this.args = args;
            this.log = log;
            UUID = Environment.GetEnvironmentVariable("UUID");

            ZAMPGUIPATH = Environment.GetEnvironmentVariable("ZAMPGUIPATH");
            if (!Directory.Exists(ZAMPGUIPATH))
            {
                throw new Exception($"directory {ZAMPGUIPATH} not found");
            }

            MARIADBBIN = Environment.GetEnvironmentVariable("MARIADBBIN");
            if (!Directory.Exists(MARIADBBIN))
            {
                throw new Exception($"directory {MARIADBBIN} not found");
            }

            CURRENT_VERS = Environment.GetEnvironmentVariable("CURRENT_VERS");
            if(string.IsNullOrEmpty(CURRENT_VERS))
            {
                throw new Exception("CURRENT_VERS not set");
            }

            ini = new IniFile(Path.Combine(ZAMPGUIPATH, "Apps", "ZampGUI2", "config.ini"));
            
        }

        public void run()
        {
            // Parametri per la connessione al server MariaDB.
            // Modifica questi valori in base alla tua configurazione.
            string dbUser = ini.GetValue("MariaDB", "username");
            string dbPass = ini.GetValue("MariaDB", "password");
            string dbHost = "localhost";

            // Creazione della stringa di connessione (senza specificare il database)
            string connectionString = $"server={dbHost};user={dbUser};password={dbPass};";

            // Definizione della cartella principale di backup, ad esempio "DatabaseBackups"
            string backupRoot = Path.Combine(ZAMPGUIPATH, "DatabaseBackups");
            if (!Directory.Exists(backupRoot))
            {
                Directory.CreateDirectory(backupRoot);
            }

            // Nome della sottocartella corrispondente alla data corrente (formato "yyyyMMdd")
            string currentDateFolder = DateTime.Now.ToString("yyyy-MM-dd");
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
                string timestamp = DateTime.Now.ToString("HH-mm-ss-fff");
                // Nome del file SQL: [nomeDatabase]_[timestamp].sql
                string fileName = $"{db}_{timestamp}.sql";
                string filePath = Path.Combine(backupDir, fileName);

                // Costruisco la stringa degli argomenti per mysqldump:
                // --add-drop-database aggiunge le istruzioni DROP/CREATE per il database;
                // --databases include il comando USE e le informazioni per la creazione del database;
                // --routines e --triggers permettono di esportare stored procedure, funzioni e trigger.
                // L'opzione --result-file permette di salvare direttamente il dump nel file specificato.
                string dumpArguments = $"-u{dbUser} -p{dbPass} -h{dbHost} --add-drop-database --databases {db} --routines --triggers --result-file=\"{filePath}\"";

                Console.WriteLine($"Starting backup database: {db}");
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = MARIADBBIN + "\\mariadb-dump.exe",
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
                        log.writeErrorLine($"Error on db {db}: {errorOutput}");
                    }
                    else
                    {
                        log.writeLine($"Backup ok for {db}");
                        log.writeLine($"New File : {filePath}");
                    }
                }
            }
        }
    }
}
