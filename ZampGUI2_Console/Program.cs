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
            args = ["wpnewinstance"];
            Environment.SetEnvironmentVariable("ZAMPGUIPATH", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full");
            Environment.SetEnvironmentVariable("UUID", "mio");
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
                case "wpnewinstance":
                    if (Debugger.IsAttached)
                    {
                        Environment.SetEnvironmentVariable("CURRENT_VERS", "84");
                        Environment.SetEnvironmentVariable("HTTPPORT", "80");
                    }

                    log.writeLine("starting wordpress automatic installation");
                    WpNewInstance runWPInstallation = new WpNewInstance(args, log);
                    runWPInstallation.run();
                    break;
                case "backupdatabases":
                    if (Debugger.IsAttached)
                    {
                        Environment.SetEnvironmentVariable("CURRENT_VERS", "84");
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

                case "wprestoreinstance":
                    if (Debugger.IsAttached)
                    {
                        //Environment.SetEnvironmentVariable("ZAMPGUIPATH", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full");
                    }
                    log.writeLine("starting wordpress restore instance");
                    break;
                case "wpdeleteinstance":
                    if (Debugger.IsAttached)
                    {
                        //Environment.SetEnvironmentVariable("ZAMPGUIPATH", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full");
                    }
                    log.writeLine("starting wordpress delete instance");
                    break;
                case "wpsaveinstance":
                    if (Debugger.IsAttached)
                    {
                        //Environment.SetEnvironmentVariable("ZAMPGUIPATH", @"C:\Users\pabloindev\Desktop\varie\portable\rollingzampv2\ZampGUI_2.0.0_full");
                    }
                    log.writeLine("starting wordpress save instance");
                    break;

                default:
                    log.writeLine($"error");
                    return 2; // codice di errore per job non riconosciuto
            }
        }
        catch (Exception ex)
        {
            // Gestione generica degli errori: scrive l'errore sullo standard error
            log.writeErrorLine("Errore durante l'esecuzione dell'applicazione: " + ex.Message);
            exitcode = 99;
        }

        return exitcode;
    }

    
}