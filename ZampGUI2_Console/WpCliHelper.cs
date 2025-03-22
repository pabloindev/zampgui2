using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZampGUI2_Console
{
    public class WpCliHelper
    {
        private static readonly Random _random = new();
        private static readonly string[] _loremWords = { "Lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing" };
        public static readonly string[] _categories = { "category1", "category2", "category3", "category4", "category5", "category6"};
        private string wpCliPath;
        private Logging log;
        public string lastOutput = "";

        public WpCliHelper(string wpCliPath, Logging log)
        {
            this.wpCliPath = wpCliPath;
            this.log = log;
        }

        public string GenerateRandomTitle()
        {
            return string.Join(" ", _loremWords
                .OrderBy(x => _random.Next())
                .Take(3))
                .Trim() + _random.Next(1000);
        }

        public string GenerateRandomContent(int paragraphs = 4)
        {
            return string.Join("\n\n", Enumerable.Range(0, paragraphs)
                .Select(_ => string.Join(" ", Enumerable.Range(0, 20)
                    .Select(__ => _loremWords[_random.Next(_loremWords.Length)]))));
        }
        public string GenerateRandomExcerpt(int paragraphs = 1)
        {
            return string.Join("\n\n", Enumerable.Range(0, paragraphs)
                .Select(_ => string.Join(" ", Enumerable.Range(0, 20)
                    .Select(__ => _loremWords[_random.Next(_loremWords.Length)]))));
        }

        public int GetTotalCategories()
        {
            return _categories.Length;
        }
        public string GetCategory()
        {
            Random random = new Random();
            int index = random.Next(_categories.Length);
            return _categories[index];
        }

        public string GetRandomDateForPost()
        {
            Random random = new Random();

            // Data massima: momento attuale
            DateTime dataMassima = DateTime.Now;

            // Data minima: un anno prima della data massima
            DateTime dataMinima = dataMassima.AddYears(-1);

            // Differenza in secondi tra le due date
            TimeSpan intervallo = dataMassima - dataMinima;
            int secondiTotali = (int)intervallo.TotalSeconds;

            // Secondi casuali da aggiungere alla data minima
            int secondiCasuali = random.Next(secondiTotali);

            // Genera la data casuale
            DateTime dataCasuale = dataMinima.AddSeconds(secondiCasuali);

            // Formatta la data come richiesto
            return dataCasuale.ToString("yyyy-MM-dd HH:mm:ss");
        }


        // Funzione locale per eseguire i comandi wp-cli e restituire il codice di uscita
        public int RunWpCommand(string arguments, string workingDirectory)
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
                lastOutput = output;
                if (exitCode != 0)
                {
                    log.writeErrorLine($"Error while executing: wp {arguments}");
                    log.writeErrorLine($"Output: {output}");
                    log.writeErrorLine($"Error: {err}");
                }
                return exitCode;
            }
        }





    }
}
