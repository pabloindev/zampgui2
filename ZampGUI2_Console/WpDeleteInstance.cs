using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZampGUI2_Console
{
    class WpDeleteInstance
    {
        string serr = "";
        string[] args { get; set; }
        Logging log { get; set; }
        IniFile ini { get; set; }
        WpCliHelper wphelper { get; set; }
        string ZAMPGUIPATH { get; set; }
        string CURRENT_VERS { get; set; }
        string MARIADBBIN { get; set; }

        public WpDeleteInstance(string[] args, Logging log)
        {
            this.args = args;
            this.log = log;
            //string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "sibling_folder_name", "pippo"));

            ZAMPGUIPATH = Environment.GetEnvironmentVariable("ZAMPGUIPATH");
            CURRENT_VERS = Environment.GetEnvironmentVariable("CURRENT_VERS");
            MARIADBBIN = Environment.GetEnvironmentVariable("MARIADBBIN");

            if (!Directory.Exists(ZAMPGUIPATH))
            {
                throw new Exception($"directory {ZAMPGUIPATH} not found");
            }
            if (!int.TryParse(CURRENT_VERS, out _))
            {
                throw new Exception($"value {CURRENT_VERS} not valid");
            }
            if (!Directory.Exists(MARIADBBIN))
            {
                throw new Exception($"directory {MARIADBBIN} not found");
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
            log.writeLine("Welcome to the WordPress delete instance script!");


            string[] alldb = Helper.GetAlldb(dbHost, dbUser, dbPass);
            string[] allfolder = Helper.GetAllWPFolder(htdocs);
            string[] intersezione = alldb.Intersect(allfolder).ToArray();

            if(intersezione.Length > 0)
            {
                log.writeLine($"Found {intersezione.Length} Instances");
                foreach (string namewp in intersezione)
                {
                    log.writeLine($"{namewp}");
                }
                log.write("Enter your choice (write 'all' to delete all instances): ");
                string databaseName = Console.ReadLine().Trim();

                if(databaseName != "all")
                {
                    intersezione = intersezione.Intersect(new string[] { databaseName }).ToArray();
                }
                
                if(intersezione.Length > 0)
                {
                    foreach (string wpname in intersezione)
                    {
                        string wordpressDirectory = Path.Combine(htdocs, wpname);

                        // Cancella il database
                        bool dbDeleteSuccess = DeleteDatabase(wpname, dbUser, dbPass, dbHost);
                        if (!dbDeleteSuccess)
                        {
                            log.writeErrorLine($"Error while deleting db {wpname}");
                        }

                        // Cancella la directory
                        if (Directory.Exists(wordpressDirectory))
                        {
                            Directory.Delete(wordpressDirectory, true); // true = recursivo
                        }
                        else
                        {
                            log.writeErrorLine($"Directory {wordpressDirectory} not found.");
                        }

                        log.writeLine($"wordpress instance {wpname} deleted successfully.");
                    }
                }
                else
                {
                    log.writeErrorLine("No wordpress instance found.");
                }

            }
            else
            {
                log.writeErrorLine("No wordpress instance found.");
            }
        }

        

        private bool DeleteDatabase(string databaseName, string dbUser, string dbPass, string dbHost)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = MARIADBBIN + "\\mariadb.exe",
                    Arguments = $"-u{dbUser} -p{dbPass} -h{dbHost}  -e \"DROP DATABASE IF EXISTS `{databaseName}`\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                log.writeErrorLine($"{ex.Message}");
                return false;
            }
        }
    }
}
