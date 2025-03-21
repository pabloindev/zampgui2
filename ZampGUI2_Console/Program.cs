using System;
using System.IO;
using ZampGUI2_Console;

class Program
{
    static int Main(string[] args)
    {
        int exitcode = 0;
        Logging logging = new Logging();
        try
        {
            // Verifica se il parametro obbligatorio typeofjob è presente
            if (args == null || args.Length == 0)
            {
                Console.Error.WriteLine("Errore: missing argument");
                return 1; // codice di errore
            }

            // Inizializza il logging per il giorno corrente
            logging.logouput("Starting zampgui_console.");

            // Cancellazione dei log più vecchi di 30 giorni
            logging.CleanOldLogs(30);

            // Rileva il tipo di job dal primo parametro
            string typeOfJob = args[0].ToLower(); // trasformazione in minuscolo per un confronto uniforme
            logging.logfile($"arg[0]: {typeOfJob}");

            // Chiamata delle funzioni specifiche in base al typeofjob
            switch (typeOfJob)
            {
                case "wordpressinstallation":
                    logging.logouput("starting wordpress automatic installation");
                    RunWPInstallation runWPInstallation = new RunWPInstallation(args);
                    runWPInstallation.run();
                    break;
                case "backupdatabases":
                    logging.logouput("starting backup databases");
                    RunBackupDB runBackupDB = new RunBackupDB(args);
                    runBackupDB.run();
                    break;
                case "sqlscripts":
                    logging.logouput("starting running sql scripts");
                    RunSQLScripts runSQLScripts = new RunSQLScripts(args);
                    runSQLScripts.run();
                    break;
                default:
                    logging.logouput($"error");
                    return 2; // codice di errore per job non riconosciuto
            }
        }
        catch (Exception ex)
        {
            // Gestione generica degli errori: scrive l'errore sullo standard error
            logging.logerror("Errore durante l'esecuzione dell'applicazione: " + ex.Message);
            exitcode = 99;
        }

        return exitcode;
    }

    
}