using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        WpCliHelper wphelper { get; set; }
        string ZAMPGUIPATH { get; set; }
        string UUID { get; set; }
        string CURRENT_VERS { get; set; }
        string HTTPPORT { get; set; }
        public WpNewInstance(string[] args, Logging log)
        {
            this.args = args;
            this.log = log;
            //string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "sibling_folder_name", "pippo"));

            ZAMPGUIPATH = Environment.GetEnvironmentVariable("ZAMPGUIPATH");
            UUID = Environment.GetEnvironmentVariable("UUID");
            CURRENT_VERS = Environment.GetEnvironmentVariable("CURRENT_VERS");
            HTTPPORT = Environment.GetEnvironmentVariable("HTTPPORT");
            
            if (!Directory.Exists(ZAMPGUIPATH))
            {
                throw new Exception($"directory {ZAMPGUIPATH} not found");
            }
            if (!int.TryParse(CURRENT_VERS, out _))
            {
                throw new Exception($"value {CURRENT_VERS} not valid");
            }
            if (!Regex.IsMatch(HTTPPORT, @"^\d+$"))
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
            serr = string.IsNullOrWhiteSpace(wpEmail) ? "": Helper.ValidateEmail(wpEmail); // il valore deve rispettare certi requisiti
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
            log.writeLine("Enter the WordPress version to install. For a complete List visit: https://wordpress.org/download/releases/");
            string[] _wpvers = ini.GetValue("Wordpress", "wpvers").Split(',');
            foreach (string s in _wpvers)
            {
                log.writeLine(s);
            }
            log.write("Enter your choice (default: latest): ");
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
            Environment.SetEnvironmentVariable("PHPIniDir", phpIniDir);
            this.wphelper = new WpCliHelper(wpCliPath, log);


            // Creazione della directory di installazione (corrispondente a: cd /d %HTDOCS% & mkdir "WEBSITE_NAME")
            string siteDir = Path.Combine(htdocs, websiteName);
            log.writeLine($"Creating WordPress installation directory: {siteDir}");
            Directory.CreateDirectory(siteDir);


            // Download del core di WordPress
            log.writeLine("Downloading WordPress...");
            if (wphelper.RunWpCommand($"core download --locale={wpLocale} --version={wpVersion}", siteDir) != 0)
            {
                throw new Exception("Error while Downloading WordPress...");
            }

            // Creazione del file wp-config.php
            log.writeLine("Creating wp-config.php...");
            if (wphelper.RunWpCommand($"config create --dbname={websiteName} --dbuser={dbUser} --dbpass={dbPass} --dbhost={dbHost}", siteDir) != 0)
            {
                throw new Exception("Error while Creating wp-config.php...");
            }

            // Creazione del database
            log.writeLine("Creating database...");
            if (wphelper.RunWpCommand("db create", siteDir) != 0)
            {
                throw new Exception("Error while creating database...");
                
            }

            // Costruzione dell'URL in base alla variabile HTTPPORT (se esistente)
            string url = (!string.IsNullOrEmpty(HTTPPORT) && HTTPPORT != "80")
                         ? $"http://localhost:{HTTPPORT}/{websiteName}"
                         : $"http://localhost/{websiteName}";

            // Installazione di WordPress
            log.writeLine("Installing WordPress...");
            if (wphelper.RunWpCommand($"core install --url=\"{url}\" --title=\"{websiteName}\" --admin_user={wpUser} --admin_password={wpPassword} --admin_email={wpEmail}", siteDir) != 0)
            {
                throw new Exception("Error while Installing WordPress...");
            }

            // Aggiornamento del nickname dell'admin
            log.writeLine("Updating admin nickname...");
            if (wphelper.RunWpCommand($"user update 1 --nickname={wpNickname} --display_name={wpNickname}", siteDir) != 0)
            {
                throw new Exception("Error while Updating admin nickname...");
            }

            // Impostazione del numero di revisioni (WP_POST_REVISIONS=5)
            log.writeLine("Setting number revision to 5...");
            if (wphelper.RunWpCommand("config set WP_POST_REVISIONS 5 --raw", siteDir) != 0)
            {
                throw new Exception("Error while Setting number revision...");
            }

            // Disabilitazione degli aggiornamenti automatici se richiesto
            if (disableAutoUpdate.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                log.writeLine("Disabling automatic updates...");
                if (wphelper.RunWpCommand("config set WP_AUTO_UPDATE_CORE false --raw", siteDir) != 0)
                {
                    throw new Exception("Error while Disabling automatic updates...");

                }
            }

            // Rimozione dei plugin di default, se richiesto
            if (removeDefaultPlugins.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                log.writeLine("Removing default plugins...");
                if (wphelper.RunWpCommand("plugin delete akismet hello", siteDir) != 0)
                {
                    throw new Exception("Error while Removing default plugins...");
                }
            }

            // Rimozione dei temi di default, se richiesto
            if (removeDefaultThemes.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                log.writeLine("Removing default themes...");
                if (wphelper.RunWpCommand("theme delete --all", siteDir) != 0)
                {
                    throw new Exception("Error while Removing default themes...");
                }
            }


            if(UUID == "mio")
            {
                log.writeLine("************* MIO *************");
                //permalink
                string scommand = "option update permalink_structure /%postname%/";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "rewrite structure '/%postname%/' --hard";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }
                
                
                string sql = "UPDATE wp_options SET option_value = '/%postname%/' WHERE option_name = 'permalink_structure'";
                log.writeLine("running query " + sql);
                Helper.rundbquery(sql, dbHost, dbUser, dbPass, websiteName);

                scommand = "rewrite flush --hard";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update timezone_string \"Europe/Rome\"";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update time_format \"H:i\"";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update rss_use_excerpt 1";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update uploads_use_yearmonth_folders 0";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update thumbnail_size_w 0";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update thumbnail_size_h 0";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update medium_size_w 0";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }
                
                scommand = "option update medium_size_h 0";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update large_size_w 0";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                scommand = "option update large_size_h 0";
                log.writeLine("running wp " + scommand);
                if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                {
                    throw new Exception("Error while executing " + scommand);
                }

                
                for (int i = 0; i < WpCliHelper._categories.Length; i++)
                {
                    string category = WpCliHelper._categories[i];
                    scommand = $"term create category {category} --porcelain";
                    log.writeLine("Creting catergory with " + scommand);
                    if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                    {
                        throw new Exception("Error while executing " + scommand);
                    }
                }


                int countpost = 10;
                log.write("Add new posts? [yes/no] (default: yes): ");
                string scelta = Console.ReadLine();
                scelta = string.IsNullOrWhiteSpace(scelta) ? "yes" : scelta;
                if(scelta == "yes")
                {
                    for (int i = 0; i < countpost; i++)
                    {
                        var title = wphelper.GenerateRandomTitle();
                        var content = wphelper.GenerateRandomContent();
                        var excerpt = wphelper.GenerateRandomExcerpt();
                        string category = wphelper.GetCategory();
                        string randomdate = wphelper.GetRandomDateForPost();

                        // creo un file txt temporaneo per contenere il contenuto del post che può avere più linee
                        string path_txtcontent = Path.Combine(Path.GetTempPath(), $"postcontent_{Guid.NewGuid()}.txt");
                        System.IO.File.WriteAllText(path_txtcontent, content);

                        //scommand = $"post create --porcelain --post_status=publish --post_author=1 --post_category='{category}' --tags_input='tag{i}' --post_date='{wphelper.GetRandomDateForPost()}' --post_title=\"{title}\" --post_content=\"{content}\" --post_excerpt=\"{excerpt}\" ";
                        scommand = $"post create {path_txtcontent} --porcelain --post_status=publish --post_author=1 --post_category=\"{category}\" --tags_input=\"tag{i}\" --post_date=\"{randomdate}\" --post_title=\"{title}\" --post_excerpt=\"{excerpt}\"";
                        log.writeLine("running wp " + scommand);
                        if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                        {
                            throw new Exception("Error while executing " + scommand);
                        }
                        System.IO.File.Delete(path_txtcontent);
                    }
                }
                

                log.write("Import media? Insert path or leave blank : ");
                scelta = Console.ReadLine();
                if(Directory.Exists(scelta))
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                    foreach (var imagePath in Directory.GetFiles(scelta))
                    {
                        var extension = Path.GetExtension(imagePath).ToLower();
                        if (!allowedExtensions.Contains(extension)) continue;

                        var fileName = Path.GetFileNameWithoutExtension(imagePath);
                        scommand = $"media import \"{imagePath}\" --title=\"{fileName}\" --alt=\"{fileName}\"";
                        log.writeLine("running wp " + scommand);
                        if (wphelper.RunWpCommand(scommand, siteDir) != 0)
                        {
                            throw new Exception("Error while executing " + scommand);
                        }
                    }

                    Random random = new Random();
                    
                    // recuper la lista degli id degli attachment
                    wphelper.RunWpCommand("post list --post_type=attachment --post_mime_type=image/% --field=ID --format=ids", siteDir);
                    string ids_media = wphelper.lastOutput;

                    // recupero al lista dei post pubblicati
                    wphelper.RunWpCommand("post list --post_status=publish --post_type=post --field=ID --format=ids", siteDir);
                    string ids_post = wphelper.lastOutput;

                    if (!string.IsNullOrEmpty(ids_media) && !string.IsNullOrEmpty(ids_post))
                    {
                        string[] arrid_media = ids_media.Split(' ');
                        string[] arrid_post = ids_post.Split(' ');
                        for (int i = 0; i < arrid_post.Length; i++)
                        {
                            string postid = arrid_post[i];
                            int index = random.Next(arrid_media.Length);
                            string mediaid = arrid_media[index];
                            wphelper.RunWpCommand($"post meta update {postid} _thumbnail_id {mediaid}", siteDir);
                        }
                    }
                    
                }


                log.writeLine("************* END MIO *************");
            }

            // Messaggio finale di completamento
            log.writeLine("");
            log.writeLine("******************************************************");
            log.writeLine("WordPress installation completed successfully!");
            log.writeLine($"Website URL: \"{url}\"");
            log.writeLine($"Admin Username: {wpUser}");
            log.writeLine($"Admin Password: {wpPassword}");
            log.writeLine($"Database: {websiteName}");
            log.writeLine($"Directory: {siteDir}");
            log.writeLine("******************************************************");
            log.writeLine("Press a key to continue...");
            Console.ReadKey();
        }


        
    }
}
