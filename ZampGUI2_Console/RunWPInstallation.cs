using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZampGUI2_Console
{
    class RunWPInstallation
    {
        string[] args { get; set; }
        public RunWPInstallation(string[] args)
        {
            args = args;

            string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "sibling_folder_name", "pippo"));
        }

        public void run()
        {
            // Messaggio di benvenuto
            Console.WriteLine("Welcome to the WordPress automatic installation script!\n");

            // Richiesta del nome del sito
            Console.Write("Enter the website name: ");
            string websiteName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(websiteName))
            {
                Console.Error.WriteLine("ERROR: Website name is required.");
                Console.WriteLine("Premere un tasto per terminare...");
                Console.ReadKey();
                Environment.Exit(1);
            }
            // Validazione: il nome deve contenere solo lettere
            if (!System.Text.RegularExpressions.Regex.IsMatch(websiteName, "^[a-zA-Z]+$"))
            {
                Console.Error.WriteLine("ERROR: Website name must contain only uppercase and lowercase letters.");
                Console.WriteLine("Premere un tasto per terminare...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Richiesta delle credenziali per l'admin e altri parametri
            Console.Write("Enter the admin username (default: admin): ");
            string wpUser = Console.ReadLine();
            Console.Write("Enter the admin password (default: admin): ");
            string wpPassword = Console.ReadLine();
            Console.Write("Enter the admin email (default: admin@mail.com): ");
            string wpEmail = Console.ReadLine();
            Console.Write("Enter the admin nickname (default: admin): ");
            string wpNickname = Console.ReadLine();

            // Selettore della lingua
            Console.WriteLine("\nSelect the language for WordPress installation:");
            Console.WriteLine("1. English (United States) - en_US");
            Console.WriteLine("2. Italian (Italy) - it_IT");
            Console.WriteLine("3. French (France) - fr_FR");
            Console.WriteLine("4. Spanish (Spain) - es_ES");
            Console.WriteLine("5. German (Germany) - de_DE");
            Console.Write("Enter the number corresponding to your desired language (default: 1 - en_US): ");
            string wpLanguageInput = Console.ReadLine();

            // Mappatura della scelta sul codice locale
            string wpLocale = "en_US";
            if (wpLanguageInput == "2") wpLocale = "it_IT";
            else if (wpLanguageInput == "3") wpLocale = "fr_FR";
            else if (wpLanguageInput == "4") wpLocale = "es_ES";
            else if (wpLanguageInput == "5") wpLocale = "de_DE";

            // Altri parametri: versione e opzioni
            Console.Write("Enter the WordPress version to install (default: latest): ");
            string wpVersion = Console.ReadLine();
            Console.Write("Disable automatic updates? (default: yes) [yes/no]: ");
            string disableAutoUpdate = Console.ReadLine();
            Console.Write("Remove default plugins? (default: yes) [yes/no]: ");
            string removeDefaultPlugins = Console.ReadLine();
            Console.Write("Remove default themes? (default: yes) [yes/no]: ");
            string removeDefaultThemes = Console.ReadLine();

            // Impostazione dei valori di default se i relativi input sono vuoti
            wpUser = string.IsNullOrWhiteSpace(wpUser) ? "admin" : wpUser;
            wpPassword = string.IsNullOrWhiteSpace(wpPassword) ? "admin" : wpPassword;
            wpEmail = string.IsNullOrWhiteSpace(wpEmail) ? "admin@mail.com" : wpEmail;
            wpNickname = string.IsNullOrWhiteSpace(wpNickname) ? "admin" : wpNickname;
            wpVersion = string.IsNullOrWhiteSpace(wpVersion) ? "latest" : wpVersion;
            disableAutoUpdate = string.IsNullOrWhiteSpace(disableAutoUpdate) ? "yes" : disableAutoUpdate;
            removeDefaultPlugins = string.IsNullOrWhiteSpace(removeDefaultPlugins) ? "yes" : removeDefaultPlugins;
            removeDefaultThemes = string.IsNullOrWhiteSpace(removeDefaultThemes) ? "yes" : removeDefaultThemes;

            // Parametri fissi per la connessione al database
            string dbUser = "root";
            string dbPass = "root";
            string dbHost = "localhost";
            // dbPort è presente nel batch ma non viene usato nei comandi

            // Preparazione delle variabili di percorso, in base alla variabile di ambiente ZAMPGUIPATH
            string zampguiPath = Environment.GetEnvironmentVariable("ZAMPGUIPATH");
            if (string.IsNullOrWhiteSpace(zampguiPath))
            {
                // Se non è presente, si assegna un valore di default (modificare secondo necessità)
                zampguiPath = @"C:\Zampgui";
            }
            string wpCliPath = Path.Combine(zampguiPath, "Apps", "wp-cli");
            string mariaDbPath = Path.Combine(zampguiPath, "Apps", "mariadb", "bin");
            string currentVers = Environment.GetEnvironmentVariable("CURRENT_VERS");
            if (string.IsNullOrWhiteSpace(currentVers))
            {
                currentVers = "Apache24";
            }
            string apachePath = Path.Combine(zampguiPath, "Apps", currentVers, "Apache24", "bin");
            string phpIniDir = Path.Combine(wpCliPath, "php");
            string currentPath = Environment.GetEnvironmentVariable("PATH");
            string newPath = $"{wpCliPath};{mariaDbPath};{phpIniDir};{apachePath};{currentPath}";
            Environment.SetEnvironmentVariable("PATH", newPath);

            // Directory fissa di installazione di WordPress
            string htdocs = Path.Combine(zampguiPath, "Apps", "Apache24", "htdocs");

            // Creazione della directory di installazione (corrispondente a: cd /d %HTDOCS% & mkdir "WEBSITE_NAME")
            string siteDir = Path.Combine(htdocs, websiteName);
            Console.WriteLine($"\nCreating WordPress installation directory: {siteDir}");
            Directory.CreateDirectory(siteDir);

            // Funzione locale per eseguire i comandi wp-cli e restituire il codice di uscita
            int RunWpCommand(string arguments, string workingDirectory)
            {
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "wp";
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
            Console.WriteLine("\nDownloading WordPress...");
            if (RunWpCommand($"core download --locale={wpLocale} --version={wpVersion}", siteDir) != 0)
            {
                Console.WriteLine("Error while Downloading WordPress...");
                Console.WriteLine("Premere un tasto per terminare...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Creazione del file wp-config.php
            Console.WriteLine("\nCreating wp-config.php...");
            if (RunWpCommand($"config create --dbname={websiteName} --dbuser={dbUser} --dbpass={dbPass} --dbhost={dbHost}", siteDir) != 0)
            {
                Console.WriteLine("Error while Creating wp-config.php...");
                Console.WriteLine("Premere un tasto per terminare...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Creazione del database
            Console.WriteLine("\nCreating database...");
            if (RunWpCommand("db create", siteDir) != 0)
            {
                Console.WriteLine("Error while creating database...");
                Console.WriteLine("Premere un tasto per terminare...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Costruzione dell'URL in base alla variabile HTTPPORT (se esistente)
            string httpPort = Environment.GetEnvironmentVariable("HTTPPORT");
            string url = (!string.IsNullOrEmpty(httpPort) && httpPort != "80")
                         ? $"http://localhost:{httpPort}/{websiteName}"
                         : $"http://localhost/{websiteName}";

            // Installazione di WordPress
            Console.WriteLine("\nInstalling WordPress...");
            if (RunWpCommand($"core install --url=\"{url}\" --title=\"{websiteName}\" --admin_user={wpUser} --admin_password={wpPassword} --admin_email={wpEmail}", siteDir) != 0)
            {
                Console.WriteLine("Error while Installing WordPress...");
                Console.WriteLine("Premere un tasto per terminare...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Aggiornamento del nickname dell'admin
            Console.WriteLine("\nUpdating admin nickname...");
            if (RunWpCommand($"user update 1 --nickname={wpNickname} --display_name={wpNickname}", siteDir) != 0)
            {
                Console.WriteLine("Error while Updating admin nickname...");
                Console.WriteLine("Premere un tasto per terminare...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Impostazione del numero di revisioni (WP_POST_REVISIONS=5)
            Console.WriteLine("\nSetting number revision to 5...");
            if (RunWpCommand("config set WP_POST_REVISIONS 5 --raw", siteDir) != 0)
            {
                Console.WriteLine("Error while Setting number revision...");
                Console.WriteLine("Premere un tasto per terminare...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Disabilitazione degli aggiornamenti automatici se richiesto
            if (disableAutoUpdate.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nDisabling automatic updates...");
                if (RunWpCommand("config set WP_AUTO_UPDATE_CORE false --raw", siteDir) != 0)
                {
                    Console.WriteLine("Error while Disabling automatic updates...");
                    Console.WriteLine("Premere un tasto per terminare...");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }

            // Rimozione dei plugin di default, se richiesto
            if (removeDefaultPlugins.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nRemoving default plugins...");
                if (RunWpCommand("plugin delete akismet hello", siteDir) != 0)
                {
                    Console.WriteLine("Error while Removing default plugins...");
                    Console.WriteLine("Premere un tasto per terminare...");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }

            // Rimozione dei temi di default, se richiesto
            if (removeDefaultThemes.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nRemoving default themes...");
                if (RunWpCommand("theme delete --all", siteDir) != 0)
                {
                    Console.WriteLine("Error while Removing default themes...");
                    Console.WriteLine("Premere un tasto per terminare...");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }

            // Messaggio finale di completamento
            Console.WriteLine("\nWordPress installation completed successfully!");
            Console.WriteLine($"Website URL: \"{url}\"");
            Console.WriteLine($"Admin Username: {wpUser}");
            Console.WriteLine($"Admin Password: {wpPassword}");
            Console.WriteLine("\nPremere un tasto per terminare...");
            Console.ReadKey();
        }


        public bool nameOK(string name)
        {
            // Regex: primo carattere non numerico, seguito da caratteri alfanumerici
            Regex regex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$");

            if (!regex.IsMatch(name))
            {
                return false;
                Console.WriteLine("Error: The string must be alphanumeric and cannot start with a number.");
            }
            else
            {
                return true;
                Console.WriteLine("The string is valid.");
            }
        }
    }
}
