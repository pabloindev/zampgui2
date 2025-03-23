using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace ZampGUI2_Console
{
    class WpRestoreInstance
    {
        string serr = "";
        string[] args { get; set; }
        string[] zipfiles { get; set; }
        Logging log { get; set; }
        IniFile ini { get; set; }
        WpCliHelper wphelper { get; set; }
        string ZAMPGUIPATH { get; set; }
        string CURRENT_VERS { get; set; }
        string MARIADBBIN { get; set; }

        public WpRestoreInstance(string[] args, Logging log)
        {
            this.args = args;
            this.log = log;
            //string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "sibling_folder_name", "pippo"));

            ZAMPGUIPATH = Environment.GetEnvironmentVariable("ZAMPGUIPATH");
            MARIADBBIN = Environment.GetEnvironmentVariable("MARIADBBIN");
            CURRENT_VERS = Environment.GetEnvironmentVariable("CURRENT_VERS");

            if (!Directory.Exists(ZAMPGUIPATH))
            {
                throw new Exception($"directory ZAMPGUIPATH {ZAMPGUIPATH} not found");
            }
            if (!int.TryParse(CURRENT_VERS, out _))
            {
                throw new Exception($"value CURRENT_VERS {CURRENT_VERS} not valid");
            }

            if (!Directory.Exists(MARIADBBIN))
            {
                throw new Exception($"directory MARIADB bin {MARIADBBIN} not found");
            }

            ini = new IniFile(Path.Combine(ZAMPGUIPATH, "Apps", "ZampGUI2", "config.ini"));

            if (args.Length < 2)
            {
                throw new Exception("Error: At least one SQL file must be specified.");
            }
            this.zipfiles = args.Skip(1).ToArray();
        }

        public void run()
        {
            // Parametri fissi per la connessione al database
            string dbUser = ini.GetValue("MariaDB", "username");
            string dbPass = ini.GetValue("MariaDB", "password");
            string dbHost = "localhost";
            string htdocs = Path.Combine(ZAMPGUIPATH, "Apps", CURRENT_VERS, "Apache24", "htdocs"); // Percorso della cartella htdocs

            // Messaggio di benvenuto
            log.writeLine("Welcome to the WordPress restore script!");

            foreach (string zipFilePath in zipfiles)
            {
                //log.write("Insert path to zip file: ");
                //string zipFilePath = Console.ReadLine().Trim();

                // Verifica che il file zip esista
                if (!File.Exists(zipFilePath))
                {
                    throw new Exception($"zip file {zipFilePath} not found");
                }

                string destinationDirectory = Path.Combine(htdocs, Path.GetFileNameWithoutExtension(zipFilePath));

                // Verifica che la directory di destinazione esista
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                else
                {
                    log.write($"The directory {destinationDirectory} already exists. Are you sure you want to proceed and delete the directory? [yes/no] (default yes): ");
                    string scelta = Console.ReadLine().Trim();
                    scelta = string.IsNullOrEmpty(scelta) ? "yes" : scelta;
                    if (scelta != "yes")
                    {
                        throw new Exception($"operation aborted");
                    }

                    // Cancella i file esistenti nella directory di destinazione
                    Directory.Delete(destinationDirectory, true);
                    Directory.CreateDirectory(destinationDirectory);
                }

                // Estrazione dell'archivio ZIP
                bool extractSuccess = ExtractZipArchive(zipFilePath, destinationDirectory);
                if (!extractSuccess)
                {
                    throw new Exception($"error extracting zip file {zipFilePath}");
                }

                // Trova il file SQL di backup nella directory estratta
                string sqlFilePath = FindSqlBackupFile(destinationDirectory);
                if (string.IsNullOrEmpty(sqlFilePath))
                {
                    throw new Exception($"sql backup file not found in {destinationDirectory}");
                }

                // Estrai il nome del database dal file SQL
                string databaseName = ExtractDatabaseName(sqlFilePath);
                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new Exception($"database name not found in {sqlFilePath}");
                }

                // Restore del database
                Helper.runsqlscript(sqlFilePath, MARIADBBIN + "\\mariadb.exe", dbUser, dbPass, dbHost, log);
                File.Delete(sqlFilePath);
                //bool dbRestoreSuccess = RestoreDatabase(sqlFilePath, dbUser, dbPass, dbHost, databaseName);
                //if (!dbRestoreSuccess)
                //{
                //    throw new Exception($"error restoring database {databaseName}");
                //}

                log.writeLine("*****************************************");
                log.writeLine($"Restore completed successfully");
                log.writeLine($"directory: {destinationDirectory}");
                log.writeLine($"Database restored '{databaseName}'");
                log.writeLine("*****************************************");
            }
        }


        private bool ExtractZipArchive(string zipFilePath, string destinationDirectory)
        {
            try
            {
                // Estrazione dell'archivio ZIP
                ZipFile.ExtractToDirectory(zipFilePath, destinationDirectory);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string FindSqlBackupFile(string directory)
        {
            try
            {
                // Cerca tutti i file .sql nella directory
                string[] sqlFiles = Directory.GetFiles(directory, "*.sql", SearchOption.TopDirectoryOnly);

                if (sqlFiles.Length == 0)
                {
                    return null;
                }

                // Se c'è più di un file SQL, prendi il primo
                return sqlFiles[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string ExtractDatabaseName(string sqlFilePath)
        {
            try
            {
                // Leggi le prime righe del file SQL per trovare il nome del database
                using (StreamReader reader = new StreamReader(sqlFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Cerca l'istruzione CREATE DATABASE o DROP DATABASE
                        if (line.Contains("CREATE DATABASE") || line.Contains("DROP DATABASE"))
                        {
                            // Estrai il nome del database tra i backtick
                            int startIndex = line.IndexOf("`") + 1;
                            int endIndex = line.IndexOf("`", startIndex);

                            if (startIndex > 0 && endIndex > startIndex)
                            {
                                return line.Substring(startIndex, endIndex - startIndex);
                            }
                        }

                        // Limita la ricerca alle prime 50 righe
                        if (reader.BaseStream.Position > 5000)
                        {
                            break;
                        }
                    }
                }

                // In alternativa, usa il nome del file SQL (senza estensione)
                return Path.GetFileNameWithoutExtension(sqlFilePath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool RestoreDatabase(string sqlFilePath, string dbUser, string dbPass, string dbHost, string databaseName)
        {
            try
            {
                // Prima crea il database se non esiste
                //bool createDbSuccess = CreateDatabaseIfNotExists(databaseName);
                //if (!createDbSuccess)
                //{
                //    return false;
                //}

                // Preparazione del comando per mariadb
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = MARIADBBIN + "\\mariadb.exe",
                    Arguments = $"--database={databaseName} -u{dbUser} -p{dbPass} -h{dbHost} < \"{sqlFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    // Usa la shell per supportare il reindirizzamento dell'input (<)
                    //FileName = "cmd.exe",
                    //Arguments = $"/c mariadb --database={databaseName} < \"{sqlFilePath}\""
                };

                // Esecuzione del comando
                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        return false;
                        //Console.WriteLine($"Errore nell'esecuzione di mariadb: {error}");

                        // Prova un approccio alternativo usando il parametro -e
                        //return RestoreDatabaseAlternative(sqlFilePath, databaseName);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                //Console.WriteLine($"Errore durante il restore del database: {ex.Message}");

                // Prova un approccio alternativo se il primo fallisce
                //return RestoreDatabaseAlternative(sqlFilePath, databaseName);
            }
        }

        private bool CreateDatabaseIfNotExists(string databaseName)
        {
            try
            {
                // Preparazione del comando per creare il database se non esiste
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "mariadb",
                    Arguments = $"-e \"CREATE DATABASE IF NOT EXISTS `{databaseName}`\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Esecuzione del comando
                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine($"Errore nella creazione del database: {error}");
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la creazione del database: {ex.Message}");
                return false;
            }
        }

        private bool RestoreDatabaseAlternative(string sqlFilePath, string databaseName)
        {
            try
            {
                // Approccio alternativo usando l'opzione --execute per eseguire il file SQL
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "mariadb",
                    Arguments = $"--database={databaseName} --execute=\"source {sqlFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Esecuzione del comando
                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine($"Errore nell'esecuzione alternativa di mariadb: {error}");
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il restore alternativo del database: {ex.Message}");
                return false;
            }
        }
    }
}
