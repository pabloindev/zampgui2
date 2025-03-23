using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZampGUI2.helper
{
    public class Percorsi
    {
        IniFile config;
        public string pathBase { get; set; }

        public string[] apachephpvers
        { 
            get 
            {
                string apachephpvers = config.Read("FolderName", "apachephpvers");
                string[] result = apachephpvers.Split(new char[] {','});
                return result;
            }
        }
        public string currentvers
        {
            get
            {
                string result = config.Read("FolderName", "currentvers");
                return result;
            }
        }


        public string zampgui_ini
        {
            get
            {
                string result = Path.Combine(pathBase, "Apps", "ZampGUI2", "config.ini");
                return result.Replace("\\", "/");
            }
        }


        public string zampgui_console_exe
        {
            get
            {
                string result = Path.Combine(pathBase, "Apps", "ZampGUI2_Console", "ZampGUI2_Console.exe");
                return result;
            }
        }
        public string zampgui_console_path
        {
            get
            {
                string result = Path.Combine(pathBase, "Apps", "ZampGUI2_Console");
                return result;
            }
        }


        public bool apache_available { get { return System.IO.Directory.Exists(apache_folder.Replace("/", "\\")); } }
        public string apache_folder
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", currentvers, foldername);
                return result.Replace("\\", "/");
            }
        }
        public string apache_bin
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", currentvers, foldername, "bin");
                return result.Replace("\\", "/");
            }
        }
        public string apache_httpd_conf
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", currentvers, foldername, "conf", "httpd.conf");
                return result.Replace("\\", "/");
            }
        }
        public string apache_vhosts
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", currentvers, foldername, "conf", "extra", "httpd-vhosts.conf");
                return result.Replace("\\", "/");
            }
        }
        public string apache_htdocs
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", currentvers, foldername, "htdocs");
                return result.Replace("\\", "/");
            }
        }

        
        public bool php_available { get { return System.IO.Directory.Exists(php_path.Replace("/", "\\")); } }
        public string php_path
        {
            get
            {
                string foldername = config.Read("FolderName", "phpFolderName");
                string result = Path.Combine(pathBase, "Apps", currentvers, foldername);
                return result.Replace("\\", "/");
            }
        }
        public string php_ini
        {
            get
            {
                string foldername = config.Read("FolderName", "phpFolderName");
                string result = Path.Combine(pathBase, "Apps", currentvers, foldername, "php.ini");
                return result.Replace("\\", "/");
            }
        }

        
        public bool mariadb_available { get { return System.IO.Directory.Exists(mariadb_folder.Replace("/", "\\")); } }
        public string mariadb_folder
        {
            get
            {
                string foldername = config.Read("FolderName", "mariadbFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }
        public string mariadb_ini 
        {  
            get
            {
                string foldername = config.Read("FolderName", "mariadbFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "data", "my.ini");
                return result.Replace("\\", "/");
            }
        }
        public string mariadb_datadir
        {
            get
            {
                string foldername = config.Read("FolderName", "mariadbFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "data");
                return result.Replace("\\", "/");
            }
        }
        public string mariadb_bin
        {
            get
            {
                string foldername = config.Read("FolderName", "mariadbFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "bin");
                return result.Replace("\\", "/");
            }
        }
        public string mariadb_plugindir
        {
            get
            {
                string foldername = config.Read("FolderName", "mariadbFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "lib", "plugin");
                return result;
            }
        }
        

        public bool composer_available { get { return System.IO.Directory.Exists(composer_path.Replace("/", "\\")); } }
        public string composer_path
        {
            get
            {
                string foldername = config.Read("FolderName", "composerFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }


        public bool git_available { get { return System.IO.Directory.Exists(git_path.Replace("/", "\\")); } }
        public string git_path
        {
            get
            {
                string foldername = config.Read("FolderName", "gitFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "bin");
                return result.Replace("\\", "/");
            }
        }


        public bool sass_available { get { return System.IO.Directory.Exists(sass_path.Replace("/", "\\")); } }
        public string sass_path
        {
            get
            {
                string foldername = config.Read("FolderName", "sassFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }


        public bool node_available { get { return System.IO.Directory.Exists(node_path.Replace("/", "\\")); } }
        public string node_path
        {
            get
            {
                string foldername = config.Read("FolderName", "nodeFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }


        public bool wpcli_available { get { return System.IO.Directory.Exists(wpcli_path.Replace("/", "\\")); } }
        public string wpcli_path
        {
            get
            {
                string foldername = config.Read("FolderName", "wpcliFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }


        public Percorsi(IniFile config)
        {
            this.config = config;
            this.pathBase = config.Read("ImpostazioniGenerali", "pathBase");
        }


    }
}
