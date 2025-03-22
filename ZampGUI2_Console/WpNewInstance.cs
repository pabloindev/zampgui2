using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ZampGUI2_Console
{
    class WpNewInstance
    {
        string serr = "";
        string[] args { get; set; }
        Logging log { get; set; }
        IniFile ini { get; set; }
        string ZAMPGUIPATH { get; set; }
        string CURRENT_VERS { get; set; }
        string HTTPPORT { get; set; }
        public WpNewInstance(string[] args, Logging log)
        {
            this.args = args;
            this.log = log;
            //string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "sibling_folder_name", "pippo"));

            ZAMPGUIPATH = Environment.GetEnvironmentVariable("ZAMPGUIPATH");
            CURRENT_VERS = Environment.GetEnvironmentVariable("CURRENT_VERS");
            HTTPPORT = Environment.GetEnvironmentVariable("HTTPPORT");
            
            if (!Directory.Exists(ZAMPGUIPATH))
            {
                throw new Exception($"directory {ZAMPGUIPATH} not found");
            }
            if (Regex.IsMatch(CURRENT_VERS, @"^\d+$"))
            {
                throw new Exception($"value {CURRENT_VERS} not valid");
            }
            if (Regex.IsMatch(HTTPPORT, @"^\d+$"))
            {
                throw new Exception($"value {HTTPPORT} not valid");
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
            log.writeLine("Welcome to the WordPress automatic installation script!");

            

            // Richiesta del nome del sito
            log.write("Enter the website name: ");
            string websiteName = Console.ReadLine();
            serr = Helper.nameOK(websiteName); // il valore deve rispettare certi requisiti
            if (serr != "")
            {
                throw new Exception(serr);
            }
            if(Directory.Exists(Path.Combine(htdocs, websiteName)))
            {
                throw new Exception($"The directory {websiteName} already exists");
            }
            if(Helper.dbExist(dbHost, dbUser, dbPass, websiteName))
            {
                throw new Exception($"The database {websiteName} already exists");
            }

            
            // Richiesta delle credenziali per l'admin e altri parametri
            log.write("Enter the admin username (default: admin): ");
            string wpUser = Console.ReadLine();
            serr = Helper.nameOK(wpUser, true); // il valore deve rispettare certi requisiti
            if (serr != "")
            {
                throw new Exception(serr);
            }
            wpUser = string.IsNullOrWhiteSpace(wpUser) ? "admin" : wpUser;



            log.write("Enter the admin password (default: admin): ");
            string wpPassword = Console.ReadLine();
            wpPassword = string.IsNullOrWhiteSpace(wpPassword) ? "admin" : wpPassword;


            log.write("Enter the admin email (default: admin@mail.com): ");
            string wpEmail = Console.ReadLine();
            serr = Helper.ValidateEmail(wpEmail); // il valore deve rispettare certi requisiti
            if (serr != "")
            {
                throw new Exception(serr);
            }
            wpEmail = string.IsNullOrWhiteSpace(wpEmail) ? "admin@mail.com" : wpEmail;


            log.write("Enter the admin nickname (default: admin): ");
            string wpNickname = Console.ReadLine();
            serr = Helper.nameOK(wpNickname, true); // il valore deve rispettare certi requisiti
            if (serr != "")
            {
                throw new Exception(serr);
            }
            wpNickname = string.IsNullOrWhiteSpace(wpNickname) ? "admin" : wpNickname;

            // Selettore della lingua
            log.writeLine("Select the language for WordPress installation:");
            log.writeLine("For a complete List visit: https://wpastra.com/docs/complete-list-wordpress-locale-codes/");
            string[] wplang = ini.GetValue("Wordpress", "wplang").Split(',');
            foreach (string s in wplang) {
                log.writeLine(s);
            }
            log.write("Enter your desired language (default: en_US): ");
            string wpLanguageInput = Console.ReadLine();
            string wpLocale = string.IsNullOrWhiteSpace(wpLanguageInput) ? "en_US" : wpLanguageInput;
            

            // Altri parametri: versione e opzioni
            log.writeLine("Enter the WordPress version to install (default: latest): ");
            log.writeLine("For a complete List visit: https://wordpress.org/download/releases/");
            log.write("Enter your choice: ");
            string wpVersion = Console.ReadLine();
            wpVersion = string.IsNullOrWhiteSpace(wpVersion) ? "latest" : wpVersion;


            log.write("Disable automatic updates? (default: yes) [yes/no]: ");
            string disableAutoUpdate = Console.ReadLine();
            if (  !string.IsNullOrWhiteSpace(disableAutoUpdate) && !(new[] { "yes", "no" }.Contains(disableAutoUpdate))  )
            {
                throw new Exception($"Error, the value {disableAutoUpdate} is not valid");
            }
            disableAutoUpdate = string.IsNullOrWhiteSpace(disableAutoUpdate) ? "yes" : disableAutoUpdate;


            log.write("Remove default plugins? (default: yes) [yes/no]: ");
            string removeDefaultPlugins = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(removeDefaultPlugins) && !(new[] { "yes", "no" }.Contains(removeDefaultPlugins)))
            {
                throw new Exception($"Error, the value {removeDefaultPlugins} is not valid");
            }
            removeDefaultPlugins = string.IsNullOrWhiteSpace(removeDefaultPlugins) ? "yes" : removeDefaultPlugins;


            log.write("Remove default themes? (default: yes) [yes/no]: ");
            string removeDefaultThemes = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(removeDefaultThemes) && !(new[] { "yes", "no" }.Contains(removeDefaultThemes)))
            {
                throw new Exception($"Error, the value {removeDefaultThemes} is not valid");
            }
            removeDefaultThemes = string.IsNullOrWhiteSpace(removeDefaultThemes) ? "yes" : removeDefaultThemes;



            
            

            // Preparazione delle variabili di percorso, in base alla variabile di ambiente ZAMPGUIPATH
            string wpCliPath = Path.Combine(ZAMPGUIPATH, "Apps", "wp-cli");
            string mariaDbPath = Path.Combine(ZAMPGUIPATH, "Apps", "mariadb", "bin");
            string apachePath = Path.Combine(ZAMPGUIPATH, "Apps", CURRENT_VERS, "Apache24", "bin");
            string phpIniDir = Path.Combine(wpCliPath, "php");
            string currentPath = Environment.GetEnvironmentVariable("PATH");
            string newPath = $"{wpCliPath};{mariaDbPath};{phpIniDir};{apachePath};{currentPath}";
            Environment.SetEnvironmentVariable("PATH", newPath);

            // Directory fissa di installazione di WordPress
            

            // Creazione della directory di installazione (corrispondente a: cd /d %HTDOCS% & mkdir "WEBSITE_NAME")
            string siteDir = Path.Combine(htdocs, websiteName);
            log.writeLine($"Creating WordPress installation directory: {siteDir}");
            Directory.CreateDirectory(siteDir);

            // Funzione locale per eseguire i comandi wp-cli e restituire il codice di uscita
            int RunWpCommand(string arguments, string workingDirectory)
            {
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = wpCliPath + "\\wp.bat";
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.WorkingDirectory = workingDirectory;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string err = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    int exitCode = process.ExitCode;
                    if (exitCode != 0)
                    {
                        Console.Error.WriteLine($"\nErrore nella esecuzione del comando: wp {arguments}");
                        Console.Error.WriteLine($"Output: {output}");
                        Console.Error.WriteLine($"Error: {err}");
                    }
                    return exitCode;
                }
            }

            // Download del core di WordPress
            log.writeLine("Downloading WordPress...");
            if (RunWpCommand($"core download --locale={wpLocale} --version={wpVersion}", siteDir) != 0)
            {
                throw new Exception("Error while Downloading WordPress...");
            }

            // Creazione del file wp-config.php
            log.writeLine("Creating wp-config.php...");
            if (RunWpCommand($"config create --dbname={websiteName} --dbuser={dbUser} --dbpass={dbPass} --dbhost={dbHost}", siteDir) != 0)
            {
                throw new Exception("Error while Creating wp-config.php...");
            }

            // Creazione del database
            log.writeLine("Creating database...");
            if (RunWpCommand("db create", siteDir) != 0)
            {
                throw new Exception("Error while creating database...");
                
            }

            // Costruzione dell'URL in base alla variabile HTTPPORT (se esistente)
            string url = (!string.IsNullOrEmpty(HTTPPORT) && HTTPPORT != "80")
                         ? $"http://localhost:{HTTPPORT}/{websiteName}"
                         : $"http://localhost/{websiteName}";

            // Installazione di WordPress
            log.writeLine("Installing WordPress...");
            if (RunWpCommand($"core install --url=\"{url}\" --title=\"{websiteName}\" --admin_user={wpUser} --admin_password={wpPassword} --admin_email={wpEmail}", siteDir) != 0)
            {
                throw new Exception("Error while Installing WordPress...");
            }

            // Aggiornamento del nickname dell'admin
            log.writeLine("Updating admin nickname...");
            if (RunWpCommand($"user update 1 --nickname={wpNickname} --display_name={wpNickname}", siteDir) != 0)
            {
                throw new Exception("Error while Updating admin nickname...");
            }

            // Impostazione del numero di revisioni (WP_POST_REVISIONS=5)
            log.writeLine("Setting number revision to 5...");
            if (RunWpCommand("config set WP_POST_REVISIONS 5 --raw", siteDir) != 0)
            {
                throw new Exception("Error while Setting number revision...");
            }

            // Disabilitazione degli aggiornamenti automatici se richiesto
            if (disableAutoUpdate.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                log.writeLine("Disabling automatic updates...");
                if (RunWpCommand("config set WP_AUTO_UPDATE_CORE false --raw", siteDir) != 0)
                {
                    throw new Exception("Error while Disabling automatic updates...");

                }
            }

            // Rimozione dei plugin di default, se richiesto
            if (removeDefaultPlugins.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                log.writeLine("Removing default plugins...");
                if (RunWpCommand("plugin delete akismet hello", siteDir) != 0)
                {
                    throw new Exception("Error while Removing default plugins...");
                }
            }

            // Rimozione dei temi di default, se richiesto
            if (removeDefaultThemes.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                log.writeLine("Removing default themes...");
                if (RunWpCommand("theme delete --all", siteDir) != 0)
                {
                    throw new Exception("Error while Removing default themes...");
                }
            }

            // Messaggio finale di completamento
            log.writeLine("\nWordPress installation completed successfully!");
            log.writeLine($"Website URL: \"{url}\"");
            log.writeLine($"Admin Username: {wpUser}");
            log.writeLine($"Admin Password: {wpPassword}");
            log.writeLine($"Database: {websiteName}");
            log.writeLine($"Directory: {siteDir}");
            log.writeLine("Press a key to continue...");
            Console.ReadKey();
        }


        
    }
}
