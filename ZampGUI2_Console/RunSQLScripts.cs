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
        string CURRENT_VERS { get; set; }
        public RunSQLScripts(string[] args, Logging log)
        {
            this.args = args;
            this.log = log;
            //UUID = Environment.GetEnvironmentVariable("UUID");
            MARIADBBIN = Environment.GetEnvironmentVariable("MARIADBBIN");
            ZAMPGUIPATH = Environment.GetEnvironmentVariable("ZAMPGUIPATH");
            CURRENT_VERS = Environment.GetEnvironmentVariable("CURRENT_VERS");

            if (!Directory.Exists(ZAMPGUIPATH))
            {
                throw new Exception($"directory ZAMPGUIPATH {ZAMPGUIPATH} not found");
            }
            if (!int.TryParse(CURRENT_VERS, out _))
            {
                throw new Exception($"value CURRENT_VERS {CURRENT_VERS} not valid");
            }
            if (!Directory.Exists(MARIADBBIN))
            {
                throw new Exception($"directory MARIADB bin {MARIADBBIN} not found");
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

            log.writeLine("Starting SQL files execution on MariaDB server...");

            foreach (string sqlFile in sqlfiles)
            {
                Helper.runsqlscript(sqlFile, mariadbexe, dbUser, dbPass, dbHost, log);
            }
        }
    }

}

