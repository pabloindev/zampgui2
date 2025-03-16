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
        public string pathBase;

        public string apache_folder
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }
        public string apache_bin
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "bin");
                return result.Replace("\\", "/");
            }
        }
        public string apache_httpd_conf
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "conf", "httpd.conf");
                return result.Replace("\\", "/");
            }
        }
        public string apache_vhosts
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "conf", "extra", "httpd-vhosts.conf");
                return result.Replace("\\", "/");
            }
        }
        public string apache_htdocs
        {
            get
            {
                string foldername = config.Read("FolderName", "apacheFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "htdocs");
                return result.Replace("\\", "/");
            }
        }
        public string php_path
        {
            get
            {
                string foldername = config.Read("FolderName", "phpFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }
        public string php_ini
        {
            get
            {
                string foldername = config.Read("FolderName", "phpFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "php.ini");
                return result.Replace("\\", "/");
            }
        }
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
        public string composer_path
        {
            get
            {
                string foldername = config.Read("FolderName", "composerFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }
        public string git_path
        {
            get
            {
                string foldername = config.Read("FolderName", "gitFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername, "bin");
                return result.Replace("\\", "/");
            }
        }
        public string sass_path
        {
            get
            {
                string foldername = config.Read("FolderName", "sassFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }
        public string node_path
        {
            get
            {
                string foldername = config.Read("FolderName", "nodeFolderName");
                string result = Path.Combine(pathBase, "Apps", foldername);
                return result.Replace("\\", "/");
            }
        }
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
