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
    class WpSaveInstance
    {
        string serr = "";
        string[] args { get; set; }
        Logging log { get; set; }
        IniFile ini { get; set; }
        WpCliHelper wphelper { get; set; }
        string ZAMPGUIPATH { get; set; }
        string CURRENT_VERS { get; set; }
        string MARIADBBIN { get; set; }

        public WpSaveInstance(string[] args, Logging log)
        {
            this.args = args;
            this.log = log;
            //string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "sibling_folder_name", "pippo"));

            ZAMPGUIPATH = Environment.GetEnvironmentVariable("ZAMPGUIPATH");
            CURRENT_VERS = Environment.GetEnvironmentVariable("CURRENT_VERS");
            MARIADBBIN = Environment.GetEnvironmentVariable("MARIADBBIN");

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
        }

        public void run()
        {
            // Parametri fissi per la connessione al database
            string dbUser = ini.GetValue("MariaDB", "username");
            string dbPass = ini.GetValue("MariaDB", "password");
            string dbHost = "localhost";
            string htdocs = Path.Combine(ZAMPGUIPATH, "Apps", CURRENT_VERS, "Apache24", "htdocs"); // Percorso della cartella htdocs

            // Messaggio di benvenuto
            log.writeLine("Welcome to the WordPress backup installation script!");

            string[] alldb = Helper.GetAlldb(dbHost, dbUser, dbPass);
            string[] allfolder = Helper.GetAllWPFolder(htdocs);
            string[] intersezione = alldb.Intersect(allfolder).ToArray();

            if (intersezione.Length > 0)
            {
                log.writeLine($"Found {intersezione.Length} Instances");
                foreach (string namewp in intersezione)
                {
                    log.writeLine($"{namewp}");
                }
                log.write("Which instance would you like to bakacup?: ");
                string wpname = Console.ReadLine().Trim();
                intersezione = intersezione.Intersect(new string[] { wpname }).ToArray();

                if (intersezione.Length > 0)
                {
                    string wordpressDirectory = Path.Combine(htdocs, wpname);
                    string outputArchivePath = Path.Combine(htdocs, wpname + ".zip");

                    // Verifica dell'esistenza della directory WordPress
                    if (!Directory.Exists(wordpressDirectory))
                    {
                        throw new Exception($"Directory '{wordpressDirectory}' not found.");
                    }

                    // Percorso del file SQL di backup
                    string sqlBackupFilePath = Path.Combine(wordpressDirectory, $"{wpname}.sql");

                    // Esecuzione del backup del database
                    bool dbBackupSuccess = BackupDatabase(wpname, dbUser, dbPass, dbHost, sqlBackupFilePath);
                    if (!dbBackupSuccess)
                    {
                        throw new Exception("Database backup failed.");
                    }

                    // Creazione dell'archivio ZIP
                    bool archiveSuccess = CreateZipArchive(wordpressDirectory, outputArchivePath);
                    if (!archiveSuccess)
                    {
                        throw new Exception("ZIP archive creation failed.");
                    }

                    File.Delete(sqlBackupFilePath); // Elimina il file SQL temporaneo

                    log.writeLine($"Backup completed successfully: {outputArchivePath}");
                }
                else
                {
                    log.writeErrorLine("Instance not found");
                }
            }

        }
        

        private bool BackupDatabase(string databaseName, string dbUser, string dbPass, string dbHost, string outputFilePath)
        {
            try
            {
                // Preparazione del comando per mariadb-dump
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = MARIADBBIN + "\\mariadb-dump.exe",
                    Arguments = $"-u{dbUser} -p{dbPass} -h{dbHost} --add-drop-database --databases {databaseName} --routines --triggers --result-file=\"{outputFilePath}\"",
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
                        log.writeErrorLine($"Error while executing mariadb-dump: {error}");
                        return false;
                    }

                    // Aggiunta dell'istruzione CREATE DATABASE IF NOT EXISTS
                    //if (File.Exists(outputFilePath))
                    //{
                    //    string content = File.ReadAllText(outputFilePath);
                    //    if (!content.Contains("CREATE DATABASE IF NOT EXISTS"))
                    //    {
                    //        string newHeader = $"DROP DATABASE IF EXISTS `{databaseName}`;\n" +
                    //                          $"CREATE DATABASE IF NOT EXISTS `{databaseName}`;\n" +
                    //                          $"USE `{databaseName}`;\n\n";

                    //        // Rimuovi eventuali istruzioni simili esistenti e aggiungi le nuove
                    //        content = content.Replace($"DROP DATABASE IF EXISTS `{databaseName}`;\n", "");
                    //        content = content.Replace($"CREATE DATABASE `{databaseName}`;\n", "");
                    //        content = content.Replace($"USE `{databaseName}`;\n", "");

                    //        File.WriteAllText(outputFilePath, newHeader + content);
                    //    }
                    //}

                    return true;
                }
            }
            catch (Exception ex)
            {
                log.writeErrorLine($"Error backup: {ex.Message}");
                return false;
            }
        }

        private bool CreateZipArchive(string sourceDirectory, string zipFilePath)
        {
            try
            {
                // Rimuovi il file zip se già esiste
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                }

                // Creazione dell'archivio ZIP
                ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath, CompressionLevel.Optimal,
                    false); // includeBaseDirectory = false per non includere la cartella principale

                return true;
            }
            catch (Exception ex)
            {
                log.writeErrorLine($"Error creating ZIP archive: {ex.Message}");
                return false;
            }
        }
    }
}
