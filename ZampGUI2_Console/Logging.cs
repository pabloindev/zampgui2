using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZampGUI2_Console
{
    public class Logging
    {
        string filePath { get; set; }
        string logDirectory { get; set; }
        public Logging()
        {
            logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if(!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            
            filePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd}.txt");
        }

        public void logouput(string message, bool addNewLine = true)
        {
            string fullMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            if(addNewLine)
            {
                File.AppendAllText(filePath, fullMessage + Environment.NewLine);
                Console.WriteLine(fullMessage);
            }
            else
            {
                File.AppendAllText(filePath, fullMessage);
                Console.Write(fullMessage);
            }
        }

        public void logfile(string message)
        {
            string fullMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(filePath, fullMessage + Environment.NewLine);
        }

        public void logerror(string message, bool addNewLine = true)
        {
            string fullMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            if (addNewLine)
            {
                File.AppendAllText(filePath, fullMessage + Environment.NewLine);
                Console.Error.WriteLine(fullMessage);
            }
            else
            {
                File.AppendAllText(filePath, fullMessage);
                Console.Error.Write(fullMessage);
            }
        }




        // Funzione per la cancellazione dei log più vecchi del numero di giorni specificato
        public void CleanOldLogs(int days)
        {
            var logFiles = Directory.GetFiles(logDirectory, "log_*.txt");
            DateTime threshold = DateTime.Now.AddDays(-days);
            foreach (var file in logFiles)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < threshold)
                    {
                        fileInfo.Delete();
                    }
                }
                catch (Exception ex)
                {
                    // Se la cancellazione di un file fallisce, viene scritto l'errore e si continua
                    Console.Error.WriteLine("Error while deleting log file: " + ex.Message);
                }
            }
        }
    }
}
