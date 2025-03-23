using System;
using System.Diagnostics;
using System.IO;
using ZampGUI2_Console;

class Program
{
    static int Main(string[] args)
    {
        int exitcode = 0;
        Logging log = new Logging();

        if (Debugger.IsAttached)
        {
            args = [
                ""
            ];
            Environment.SetEnvironmentVariable("ZAMPGUIPATH", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full");
            Environment.SetEnvironmentVariable("UUID", "mio");
            Environment.SetEnvironmentVariable("CURRENT_VERS", "84");
        }

        try
        {
            // Verifica se il parametro obbligatorio typeofjob è presente
            if (args == null || args.Length == 0)
            {
                Console.Error.WriteLine("Errore: missing argument");
                return 1; // codice di errore
            }

            // Inizializza il logging per il giorno corrente
            log.writeLine("Starting zampgui_console.");

            // Cancellazione dei log più vecchi di 30 giorni
            log.CleanOldLogs(30);

            // Rileva il tipo di job dal primo parametro
            string typeOfJob = args[0].ToLower(); // trasformazione in minuscolo per un confronto uniforme
            log.writeFile($"arg[0]: {typeOfJob}");

            // Chiamata delle funzioni specifiche in base al typeofjob
            switch (typeOfJob)
            {
                case "backupdatabases":
                    if (Debugger.IsAttached)
                    {
                        Environment.SetEnvironmentVariable("MARIADBBIN", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full\Apps\mariadb\bin");
                    }
                    log.writeLine("starting backup databases");
                    RunBackupDB runBackupDB = new RunBackupDB(args, log);
                    runBackupDB.run();
                    break;
                case "sqlscripts":
                    if (Debugger.IsAttached)
                    {
                        Environment.SetEnvironmentVariable("MARIADBBIN", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full\Apps\mariadb\bin");
                    }
                    log.writeLine("starting running sql scripts");
                    RunSQLScripts runSQLScripts = new RunSQLScripts(args, log);
                    runSQLScripts.run();
                    break;
                case "wpnewinstance":
                    if (Debugger.IsAttached)
                    {
                        Environment.SetEnvironmentVariable("HTTPPORT", "80");
                    }

                    log.writeLine("starting wordpress automatic installation");
                    WpNewInstance runWPInstallation = new WpNewInstance(args, log);
                    runWPInstallation.run();
                    break;
                case "wprestoreinstance":
                    if(Debugger.IsAttached)
                    {
                        Environment.SetEnvironmentVariable("MARIADBBIN", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full\Apps\mariadb\bin");
                    }
                    log.writeLine("starting wordpress restore instance");
                    WpRestoreInstance wprestore = new WpRestoreInstance(args, log);
                    wprestore.run();
                    break;
                case "wpdeleteinstance":
                    if (Debugger.IsAttached)
                    {
                        Environment.SetEnvironmentVariable("MARIADBBIN", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full\Apps\mariadb\bin");
                    }
                    log.writeLine("starting wordpress delete instance");
                    WpDeleteInstance wpdelete = new WpDeleteInstance(args, log);
                    wpdelete.run();
                    break;
                case "wpsaveinstance":
                    if (Debugger.IsAttached)
                    {
                        Environment.SetEnvironmentVariable("MARIADBBIN", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full\Apps\mariadb\bin");
                    }
                    log.writeLine("starting wordpress save instance");
                    WpSaveInstance wpsave = new WpSaveInstance(args, log);
                    wpsave.run();
                    break;

                default:
                    log.writeLine($"error");
                    return 2; // codice di errore per job non riconosciuto
            }
        }
        catch (Exception ex)
        {
            // Gestione generica degli errori: scrive l'errore sullo standard error
            log.writeErrorLine(ex.Message);
            exitcode = 99;
        }

        log.writeLine("Press a key to continue...");
        Console.ReadKey();
        return exitcode;
    }

    
}