using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZampGUI2_Run
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!IsDotNet8Installed())
            {
                Console.WriteLine(".NET 8.0 framework is not installed. You need to download it to run the application.");
                Console.WriteLine("Press ENTER to open the download link...");
                Console.ReadLine();
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://dotnet.microsoft.com/en-us/download/dotnet/8.0",
                    UseShellExecute = true
                });
                return;
            }

            StartApplication();
        }

        static bool IsDotNet8Installed()
        {
            string dotnetPath = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\dotnet\shared\Microsoft.NETCore.App");
            if (Directory.Exists(dotnetPath))
            {
                foreach (var dir in Directory.GetDirectories(dotnetPath))
                {
                    if (dir.Contains("8.0."))
                        return true;
                }
            }
            return false;
        }

        static void StartApplication()
        {
            try
            {
                string appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Apps", "ZampGUI2", "ZampGUI2.exe");
                //Process.Start(appPath);
                var p = new System.Diagnostics.Process();
                //p.StartInfo.FileName = System.IO.Path.Combine(dir, "Apps", "ZampGUI", "ZampGUI.exe");
                p.StartInfo.FileName = appPath;
                p.StartInfo.Arguments = "/secondary /minimized";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = false;
                p.Start();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error launching the application: " + ex.Message);
            }
        }
    }
}
