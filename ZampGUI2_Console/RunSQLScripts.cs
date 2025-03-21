using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZampGUI2_Console
{
    class RunSQLScripts
    {
        string[] args { get; set; }
        public RunSQLScripts(string[] args)
        {
            args = args;
        }

        public void run()
        {
            // Connection parameters for the MariaDB server.
            string dbUser = "root";
            string dbPass = "root";
            string dbHost = "localhost";

            Console.WriteLine("Starting SQL files execution on MariaDB server...");

            foreach (string sqlFile in sqlFiles)
            {
                if (!File.Exists(sqlFile))
                {
                    Console.Error.WriteLine($"Error: File '{sqlFile}' does not exist.");
                    continue;
                }

                Console.WriteLine($"Executing SQL file: {sqlFile}");

                // Create a temporary batch file to handle the redirection
                string batchFilePath = Path.Combine(Path.GetTempPath(), $"mariadb_script_{Guid.NewGuid()}.cmd");

                // Create the batch file content
                string batchContent = $"\"{mariadbexe}\" -u{dbUser} -p{dbPass} -h{dbHost} {dbName} < \"{sqlfile}\"";
                File.WriteAllText(batchFilePath, batchContent);

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{batchFilePath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    // Clean up the temporary batch file
                    try { File.Delete(batchFilePath); } catch { /* Ignore cleanup errors */ }

                    if (process.ExitCode != 0)
                    {
                        ch.LogError($"Error executing file: {sqlfile}. Exit code: {process.ExitCode}");
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            ch.LogError("Error details: " + error);
                        }
                    }
                    else
                    {
                        ch.LogMessage($"Successfully executed file: {sqlfile}.");
                        if (!string.IsNullOrWhiteSpace(output))
                        {
                            ch.LogMessage("Client output: " + output);
                        }
                    }
                }
            }

        }
    }

}
}
