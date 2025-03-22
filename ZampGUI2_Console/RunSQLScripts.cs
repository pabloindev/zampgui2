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
        string[] sqlfiles { get; set; }
        Logging log { get; set; }
        IniFile ini { get; set; }
        string ZAMPGUIPATH { get; set; }
        string MARIADBBIN { get; set; }
        public RunSQLScripts(string[] args, Logging log)
        {
            this.args = args;
            this.log = log;
            ZAMPGUIPATH = Environment.GetEnvironmentVariable("ZAMPGUIPATH");

            if (!Directory.Exists(ZAMPGUIPATH))
            {
                throw new Exception($"directory {ZAMPGUIPATH} not found");
            }

            MARIADBBIN = Environment.GetEnvironmentVariable("MARIADBBIN");

            if (!Directory.Exists(MARIADBBIN))
            {
                throw new Exception($"directory {MARIADBBIN} not found");
            }

            ini = new IniFile(Path.Combine(ZAMPGUIPATH, "Apps", "ZampGUI2", "config.ini"));

            if(args.Length < 2)
            {
                throw new Exception("Error: At least one SQL file must be specified.");
            }
            sqlfiles = args.Skip(1).ToArray();
        }

        public void run()
        {
            // Connection parameters for the MariaDB server.
            string dbUser = ini.GetValue("MariaDB", "username");
            string dbPass = ini.GetValue("MariaDB", "password");
            string dbHost = "localhost";
            string mariadbexe = Path.Combine(MARIADBBIN, "mariadb.exe");
            //string combinedPaths = string.Join(" ", args.Select(p => $"\"{p}\""));

            Console.WriteLine("Starting SQL files execution on MariaDB server...");

            foreach (string sqlFile in sqlfiles)
            {
                if (!File.Exists(sqlFile))
                {
                    throw new Exception($"Error: File '{sqlFile}' does not exist.");
                }

                log.writeLine($"Executing SQL file: {sqlFile}");

                // Create a temporary batch file to handle the redirection
                string batchFilePath = Path.Combine(Path.GetTempPath(), $"mariadb_script_{Guid.NewGuid()}.cmd");

                // Create the batch file content
                string batchContent = $"\"{mariadbexe}\" -u{dbUser} -p{dbPass} -h{dbHost} < \"{sqlFile}\"";
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
                        log.writeError($"Error executing file: {sqlFile}. Exit code: {process.ExitCode}");
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            log.writeError("Error details: " + error);
                        }
                    }
                    else
                    {
                        log.writeLine($"Successfully executed file: {sqlFile}.");
                        if (!string.IsNullOrWhiteSpace(output))
                        {
                            log.writeLine("Client output: " + output);
                        }
                    }
                }
            }

        }
    }

}

