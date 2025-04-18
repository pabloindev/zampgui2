
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Windows.Forms;
using ZampGUI2.helper;
using ZampGUI2.RenderingGrafico;

namespace ZampGUI2
{
    public partial class FormMain : Form
    {
        IniFile config;
        Helper helper;
        Percorsi percorsi;
        string pathini;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public FormMain()
        {
            InitializeComponent();

            // reading file ini
            pathini = Path.Combine(Application.StartupPath, "config.ini");
            config = new IniFile(pathini);

            // se � il primo lancio devo aggiornare il file ini aggiungendo un guid
            if (config.Read("ImpostazioniGenerali", "uuid") == "")
            {
                string guid = Guid.NewGuid().ToString();
                config.Write("ImpostazioniGenerali", "uuid", guid);
                config.Save();
                config = new IniFile(pathini);
            }

            // sono nella sezione di debug
            if (config.Read("ImpostazioniGenerali", "detachZampGUI") != "true")
            {
                //sono nella modalit� reale quindi devo verificare se la cartella di root di zampgui deve essere modificata
                string pathBase = Path.GetFullPath(Application.ExecutablePath + "\\..\\..\\..\\").Replace("\\", "/").Trim('/');
                if (pathBase != config.Read("ImpostazioniGenerali", "pathBase"))
                {
                    config.Write("ImpostazioniGenerali", "pathBase", pathBase);
                    config.Save();
                    config = new IniFile(pathini);
                }
            }

            percorsi = new Percorsi(config);
            helper = new Helper(config, this);


            // controllo se le porte di apache e mariadb sono in uso
            int portToCheck = Convert.ToInt32(config.Read("Porte", "httpPort"));
            if (!helper.processo_in_esecuzione("httpd") && !helper.IsPortAvailable(portToCheck))
            {
                MessageBox.Show($"Apache port {portToCheck} is in use", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            portToCheck = Convert.ToInt32(config.Read("Porte", "mariadbPort"));
            if (!helper.processo_in_esecuzione("mariadbd") && !helper.IsPortAvailable(portToCheck))
            {
                MessageBox.Show($"MariaDB port {portToCheck} is in use", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            //faccio un breve check per verificare se esistono le cartelle per apache php e mariadb
            if (!percorsi.apache_available || !percorsi.php_available || !percorsi.mariadb_available)
            {
                MessageBox.Show("Unable to find apache, php or mariadb folder");
                System.Windows.Forms.Application.Exit();
            }

            // tolgo la possibilit� di editare il file se non sono io
            if (config.Read("ImpostazioniGenerali", "uuid") != "mio")
            {
                zampGUIIniToolStripMenuItem.Visible = false;
            }

            // *****************************************************************
            //prosegue con operazioni eseguite nel metodo "FormMain_Load()"
            // *****************************************************************
        }

        private void disableControl()
        {
            btnStartStopApache.Enabled = false;
            btnStartStopMariadb.Enabled = false;
            menuStrip1.Enabled = false;
        }
        private void enableControl()
        {
            btnStartStopApache.Enabled = true;
            btnStartStopMariadb.Enabled = true;
            menuStrip1.Enabled = true;
        }

        private void inizializzaComponenti_dacodice()
        {
            //DarkModeManager.EnableDarkMode(this);

            // Imposta lo sfondo del form
            menuStrip1.Renderer = new RenderingGrafico.DarkMenuRenderer(); // Personalizza il rendering
            this.Text = "ZampGUI v" + config.Read("ImpostazioniGenerali", "currentVersion");


            // sezione programmi opzinali
            if (!percorsi.wpcli_available) //se la cartella non esiste non ha senso mostrare questa voce di menu
            {
                wordpressScriptToolStripMenuItem.Visible = false;
                wordpressSaveInstanceToolStripMenuItem.Visible = false;
                wordpressRestoreInstanceToolStripMenuItem.Visible = false;
                wordpressDeleteInstanceToolStripMenuItem.Visible = false;
                label_wpcli.Visible = false;
            }
            if (!percorsi.composer_available) //se la cartella non esiste non ha senso mostrare questa voce di menu
            {
                label_composer.Visible = false;
            }
            if (!percorsi.git_available) //se la cartella non esiste non ha senso mostrare questa voce di menu
            {
                label_git.Visible = false;
            }
            if (!percorsi.sass_available) //se la cartella non esiste non ha senso mostrare questa voce di menu
            {
                label_sass.Visible = false;
            }
            if (!percorsi.node_available) //se la cartella non esiste non ha senso mostrare questa voce di menu
            {
                label_node.Visible = false;
            }
            if (!percorsi.wpcli_available) //se la cartella non esiste non ha senso mostrare questa voce di menu
            {
                label_wpcli.Visible = false;
            }


            // imposto il check che mi dice se sono abilitati gli update
            checkForUpdatesAtStartupToolStripMenuItem.Checked = config.Read("ImpostazioniGenerali", "checkLastVersionApp") == "true";
            if (checkForUpdatesAtStartupToolStripMenuItem.Checked)
            {
                checkForUpdatesAtStartupToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                checkForUpdatesAtStartupToolStripMenuItem.CheckState = CheckState.Unchecked;
            }


            // se non sono io nascondo voci menu temporanee
            if(config.Read("ImpostazioniGenerali", "uuid") != "mio")
            {
                wordpressRestoreInstanceToolStripMenuItem.Visible = false;
                wordpressDeleteInstanceToolStripMenuItem.Visible = false;
                wordpressSaveInstanceToolStripMenuItem.Visible = false;
            }


            // se ho una sola versione disponibile � inutile mostrare la voce di menu che mi permette di cambiare versione
            int countfolder = 0;
            string[] arrfolder = config.Read("FolderName", "apachephpvers").Split(',');
            foreach (string s in arrfolder)
            {
                if (Directory.Exists(Path.Combine(percorsi.pathBase, "Apps", s)))
                    countfolder++;
            }
            if (countfolder == 1)
            {
                changeVersionApachePHPToolStripMenuItem.Visible = false;
            }

        }
        private void controllaAggiornamenti(bool showDialogBox = false)
        {
            string msg = "";
            (bool esisteUpdate, string url_latest_vers) = helper.checkUpdateSoftware();
            if (esisteUpdate)
            {
                msg = "New Version available at: " + url_latest_vers;
                toolStripStatusLabel1.Text = msg;
                if (showDialogBox)
                {
                    string text = "New Version available";
                    string linkText = "Click here";
                    string linkUrl = url_latest_vers;
                    helper.MostraDialogoConTestoELink(text, linkText, linkUrl);
                    //MessageBox.Show(msg);
                }
            }
            else
            {
                msg = "Ready";
                toolStripStatusLabel1.Text = msg;
                if (showDialogBox)
                {
                    MessageBox.Show("No update available");
                }
            }
        }
        private void refreshForm(bool primoCaricamentoForm = false)
        {
            // update form 
            label_path.Text = "path: " + config.Read("ImpostazioniGenerali", "pathBase");
            label_apache.Text = "apache: " + config.Read("SoftwareVersion", "apache_vers") + " (port " + config.Read("Porte", "httpPort") + ")";
            label_php.Text = "php: " + config.Read("php_vers", config.Read("FolderName", "currentvers"));
            label_mariadb.Text = "mariadb: " + config.Read("SoftwareVersion", "mariadb_vers");
            label_composer.Text = "composer: " + config.Read("SoftwareVersion", "composer_vers");
            label_git.Text = "git: " + config.Read("SoftwareVersion", "git_vers");
            label_node.Text = "nodejs: " + config.Read("SoftwareVersion", "node_vers");
            label_sass.Text = "sass: " + config.Read("SoftwareVersion", "sass_vers");
            label_wpcli.Text = "wp cli: " + config.Read("SoftwareVersion", "wp_cli_vers");
            label_uuid.Text = "uuid: " + config.Read("ImpostazioniGenerali", "uuid");

            refreshFormProcess();

            if (primoCaricamentoForm)
                controllaAggiornamenti();
        }
        public void refreshFormProcess()
        {
            // aggiorno le immaginette
            bool hhtpd_in_esecuzione = helper.processo_in_esecuzione("httpd");
            bool mariadb_in_esecuzione = helper.processo_in_esecuzione("mariadbd");
            if (hhtpd_in_esecuzione)
            {
                pictureBox_apache.Image = Properties.Resources._checked;
                btnStartStopApache.Text = "Stop Apache";
            }
            else
            {
                pictureBox_apache.Image = Properties.Resources.cancel;
                btnStartStopApache.Text = "Start Apache";
            }

            if (mariadb_in_esecuzione)
            {
                pictureBox_mariadb.Image = Properties.Resources._checked;
                btnStartStopMariadb.Text = "Stop MariaDB";
            }
            else
            {
                pictureBox_mariadb.Image = Properties.Resources.cancel;
                btnStartStopMariadb.Text = "Start MariaDB";
            }
        }
        private async void gestione_processo_wrap(string nome_processo, bool? forzaStart = null, bool? forzaStop = null)
        {
            disableControl();

            if (helper.processo_in_esecuzione(nome_processo))
            {
                var (success, messageStop) = helper.processo_stop(nome_processo);
                textBoxOuput.AppendText(messageStop);
            }
            else
            {
                helper.processo_start(nome_processo);
            }

            // Ritarda l'esecuzione di 1 secondo senza bloccare l'interfaccia utente
            await Task.Delay(3000);
            refreshForm();
            enableControl();
        }


        #region eventi
        private void FormMain_Load(object sender, EventArgs e)
        {
            inizializzaComponenti_dacodice();
            refreshForm(true);
        }
        private void btnStartStopMariadb_Click(object sender, EventArgs e)
        {
            gestione_processo_wrap("mariadbd");
        }
        private void btnStartStopApache_Click(object sender, EventArgs e)
        {
            gestione_processo_wrap("httpd");
        }
        private void openConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> arrpath = new List<string>();
            arrpath.Add(percorsi.apache_bin.Replace("/", "\\"));
            arrpath.Add(percorsi.mariadb_bin.Replace("/", "\\"));
            arrpath.Add(percorsi.php_path.Replace("/", "\\"));

            if (percorsi.composer_available)
            {
                arrpath.Add(percorsi.composer_path.Replace("/", "\\"));
            }
            if (percorsi.git_available)
            {
                arrpath.Add(percorsi.git_path.Replace("/", "\\"));
            }
            if (percorsi.node_available)
            {
                arrpath.Add(percorsi.node_path.Replace("/", "\\"));
            }
            if (percorsi.sass_available)
            {
                arrpath.Add(percorsi.sass_path.Replace("/", "\\"));
            }
            if (percorsi.wpcli_available)
            {
                arrpath.Add(percorsi.wpcli_path.Replace("/", "\\"));
            }

            var envVars = new Dictionary<string, string>
            {
                { "ZAMPGUI_PATH", config.Read("ImpostazioniGenerali", "pathBase")},
                { "PHPIniDir", percorsi.php_path},
                { "HTTP_PORT", config.Read("Porte", "httpPort")},
                { "PATH", string.Join( ";", arrpath.ToArray()) + ";" + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine)}
            };
            string startingDirectory = config.Read("ImpostazioniGenerali", "pathBase");
            string messaggioPersonalizzato = "---------------- ZAMPGUI Console ----------------";
            foreach (string path in arrpath)
            {
                messaggioPersonalizzato += "\n" + "- " + path;
            }
            messaggioPersonalizzato += "\n" + "------------------------------------------------";
            messaggioPersonalizzato += "\n" + "- Run 'httpd -v' to see apache version ";
            messaggioPersonalizzato += "\n" + "- Run 'mariadb --version' to see mariadb version ";
            messaggioPersonalizzato += "\n" + "- Run 'php -v' to see php version ";
            if (percorsi.composer_available)
            {
                messaggioPersonalizzato += "\n" + "- Run 'composer --version' to see composer version ";
            }
            if (percorsi.git_available)
            {
                messaggioPersonalizzato += "\n" + "- Run 'git -v' to see git version ";
            }
            if (percorsi.node_available)
            {
                messaggioPersonalizzato += "\n" + "- Run 'node -v' to see node version ";
            }
            if (percorsi.sass_available)
            {
                messaggioPersonalizzato += "\n" + "- Run 'sass --version' to see sass version ";
            }
            if (percorsi.wpcli_available)
            {
                messaggioPersonalizzato += "\n" + "- Run 'wp --info' to see wpcli version ";
            }
            messaggioPersonalizzato += "\n" + "------------------------------------------------";

            helper.OpenConsole(startingDirectory, envVars, messaggioPersonalizzato);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void apacheHttpdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helper.openFileWithEditor(config.Read("ImpostazioniGenerali", "editor"), percorsi.apache_httpd_conf);
        }
        private void apacheHttpdvhostsconfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helper.openFileWithEditor(config.Read("ImpostazioniGenerali", "editor"), percorsi.apache_vhosts);
        }
        private void phpiniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helper.openFileWithEditor(config.Read("ImpostazioniGenerali", "editor"), percorsi.php_ini);
        }
        private void mariadbiniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helper.openFileWithEditor(config.Read("ImpostazioniGenerali", "editor"), percorsi.mariadb_ini);
        }
        private void hostsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helper.openFileWithEditor(config.Read("ImpostazioniGenerali", "editor"), @"C:\Windows\System32\drivers\etc\hosts", true);
        }
        private void infophpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string porta = config.Read("Porte", "httpPort");
                Process.Start(new ProcessStartInfo
                {
                    FileName = "http://localhost" + (porta != "80" ? ":" + porta : "") + "/info.php",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening URL: {ex.Message}");
            }
        }
        private void phpmyadminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string porta = config.Read("Porte", "httpPort");
                Process.Start(new ProcessStartInfo
                {
                    FileName = "http://localhost" + (porta != "80" ? ":" + porta : "") + "/pma",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening URL: {ex.Message}");
            }
        }
        private void docsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = config.Read("ImpostazioniGenerali", "urlDocse"),
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening URL: {ex.Message}");
            }
        }
        private void checkUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controllaAggiornamenti(true);
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "Zamgui version " + config.Read("ImpostazioniGenerali", "currentVersion") + " \n(Wamp with apache php and mariadb)";
            string linkText = "Home page";
            string linkUrl = config.Read("ImpostazioniGenerali", "urlHomepage");

            helper.MostraDialogoConTestoELink(text, linkText, linkUrl);
        }
        private void editHTTPPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAsk frm2 = new FormAsk("http_port", config);
            DialogResult dr = frm2.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                string valoreInserito = frm2.valoreInserito;
                config.Write("Porte", "httpPort", valoreInserito);
                config.Save();
                config = new IniFile(pathini);
                MessageBox.Show("To make the changes effective please stop apache, mariadb and close and reopen the application");
            }
            frm2.Close();
        }
        private void changeDefaultEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAsk frm2 = new FormAsk("editor", config);
            DialogResult dr = frm2.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                string valoreInserito = frm2.valoreInserito;
                config.Write("ImpostazioniGenerali", "editor", valoreInserito);
                config.Save();
                config = new IniFile(pathini);
            }
            frm2.Close();
        }

        private void apacheFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", percorsi.apache_folder.Replace("/", "\\"));
        }
        private void pHPFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", percorsi.php_path.Replace("/", "\\"));
        }
        private void mariaDBFOLDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", percorsi.mariadb_folder.Replace("/", "\\"));
        }
        private void apacheHtdocsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", percorsi.apache_htdocs.Replace("/", "\\"));
        }
        private void changeVersionApachePHPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (string s in config.Read("FolderName", "apachephpvers").Split(','))
            {
                list.Add(s);
            }

            FormSelectVers frm2 = new FormSelectVers(list, config.Read("FolderName", "currentvers"));
            DialogResult dr = frm2.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                string valoreInserito = frm2.selectedValue;
                config.Write("FolderName", "currentvers", valoreInserito);
                config.Save();
                config = new IniFile(pathini);
                MessageBox.Show("To make the changes effective please stop apache, mariadb and close and reopen the application");
            }
            frm2.Close();
        }
        private void checkForUpdatesAtStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config.Write("ImpostazioniGenerali", "checkLastVersionApp", checkForUpdatesAtStartupToolStripMenuItem.Checked ? "true" : "false");
            if (checkForUpdatesAtStartupToolStripMenuItem.Checked)
            {
                checkForUpdatesAtStartupToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                checkForUpdatesAtStartupToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            config.Save();
            config = new IniFile(pathini);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            //Per la barra del titolo (title bar) scura

            base.OnHandleCreated(e);

            // Attiva la dark mode per la barra del titolo
            DarkModeHelper.SetDarkMode(this.Handle, true);

            // Imposta un colore personalizzato per la barra del titolo
            DarkModeHelper.SetCaptionColor(this.Handle, Color.FromArgb(32, 32, 32));
        }
        private void timer_refreshFormProcess_Tick(object sender, EventArgs e)
        {
            refreshFormProcess();
        }
        private void zampGUIIniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helper.openFileWithEditor(config.Read("ImpostazioniGenerali", "editor"), percorsi.zampgui_ini);
        }


        private void wordpressNewInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!helper.processo_in_esecuzione("httpd") || !helper.processo_in_esecuzione("mariadbd"))
            {
                MessageBox.Show("Please start apache and mariadb", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var envVars = new Dictionary<string, string>
            {
                { "ZAMPGUIPATH", percorsi.pathBase},
                { "UUID", config.Read("ImpostazioniGenerali", "uuid")},
                { "CURRENT_VERS", config.Read("FolderName", "currentvers")},
                { "HTTPPORT", config.Read("Porte", "httpPort")}
            };

            bool result = helper.runZampGUI_Console("wpnewinstance", envVars, new string[] { });
        }
        private void backupDatabasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!helper.processo_in_esecuzione("httpd") || !helper.processo_in_esecuzione("mariadbd"))
            {
                MessageBox.Show("Please start apache and mariadb", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var envVars = new Dictionary<string, string>
            {
                { "ZAMPGUIPATH", percorsi.pathBase},
                { "CURRENT_VERS", config.Read("FolderName", "currentvers")},
                { "MARIADBBIN", percorsi.mariadb_bin}
            };

            bool result = helper.runZampGUI_Console("backupdatabases", envVars, new string[] { });
        }
        private void runSqlScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!helper.processo_in_esecuzione("httpd") || !helper.processo_in_esecuzione("mariadbd"))
            {
                MessageBox.Show("Please start apache and mariadb", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Creazione dell'oggetto OpenFileDialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Configurazione della finestra di dialogo
                openFileDialog.Title = "Select SQL file";
                //openFileDialog.Filter = "File SQL (*.sql)|*.sql|Tutti i file (*.*)|*.*";
                openFileDialog.Filter = "File SQL (*.sql)|*.sql";
                openFileDialog.Multiselect = true; // Permette la selezione di pi� file
                openFileDialog.RestoreDirectory = true; // Ripristina la directory iniziale dopo la chiusura

                // Mostra la finestra di dialogo e verifica se l'utente ha premuto OK
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Recupera i percorsi dei file selezionati
                    string[] selectedFiles = openFileDialog.FileNames;

                    // Creazione di una lista per memorizzare i file selezionati
                    List<string> fileList = new List<string>(selectedFiles);

                    if (fileList.Count() > 0)
                    {
                        var envVars = new Dictionary<string, string>
                        {
                            { "ZAMPGUIPATH", percorsi.pathBase},
                            { "CURRENT_VERS", config.Read("FolderName", "currentvers")},
                            { "MARIADBBIN", percorsi.mariadb_bin}
                        };
                        bool result = helper.runZampGUI_Console("sqlscripts", envVars, fileList.ToArray());
                    }
                }
            }
        }
        private void wordpressRestoreInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!helper.processo_in_esecuzione("httpd") || !helper.processo_in_esecuzione("mariadbd"))
            {
                MessageBox.Show("Please start apache and mariadb", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Configurazione della finestra di dialogo
                openFileDialog.Title = "Select ZIP File";
                //openFileDialog.Filter = "File SQL (*.sql)|*.sql|Tutti i file (*.*)|*.*";
                openFileDialog.Filter = "ZIP File (.zip)|*.zip";
                openFileDialog.Multiselect = true; // Permette la selezione di pi� file
                openFileDialog.RestoreDirectory = true; // Ripristina la directory iniziale dopo la chiusura

                // Mostra la finestra di dialogo e verifica se l'utente ha premuto OK
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Recupera i percorsi dei file selezionati
                    string[] selectedFiles = openFileDialog.FileNames;

                    // Creazione di una lista per memorizzare i file selezionati
                    List<string> fileList = new List<string>(selectedFiles);

                    if (fileList.Count() > 0)
                    {
                        var envVars = new Dictionary<string, string>
                        {
                            { "ZAMPGUIPATH", percorsi.pathBase},
                            { "CURRENT_VERS", config.Read("FolderName", "currentvers")},
                            { "MARIADBBIN", percorsi.mariadb_bin}
                        };
                        bool result = helper.runZampGUI_Console("wprestoreinstance", envVars, fileList.ToArray());
                    }
                }
            }
            //var envVars = new Dictionary<string, string>
            //{
            //    { "ZAMPGUIPATH", percorsi.pathBase},
            //    { "CURRENT_VERS", config.Read("FolderName", "currentvers")},
            //    { "MARIADBBIN", percorsi.mariadb_bin}
            //};
            //bool result = helper.runZampGUI_Console("wprestoreinstance", envVars, new string[] { });
        }
        private void wordpressDeleteInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!helper.processo_in_esecuzione("httpd") || !helper.processo_in_esecuzione("mariadbd"))
            {
                MessageBox.Show("Please start apache and mariadb", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var envVars = new Dictionary<string, string>
            {
                { "ZAMPGUIPATH", percorsi.pathBase},
                { "CURRENT_VERS", config.Read("FolderName", "currentvers")},
                { "MARIADBBIN", percorsi.mariadb_bin}
            };
            bool result = helper.runZampGUI_Console("wpdeleteinstance", envVars, new string[] { });
        }
        private void wordpressSaveInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!helper.processo_in_esecuzione("httpd") || !helper.processo_in_esecuzione("mariadbd"))
            {
                MessageBox.Show("Please start apache and mariadb", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var envVars = new Dictionary<string, string>
            {
                { "ZAMPGUIPATH", percorsi.pathBase},
                { "CURRENT_VERS", config.Read("FolderName", "currentvers")},
                { "MARIADBBIN", percorsi.mariadb_bin}
            };
            bool result = helper.runZampGUI_Console("wpsaveinstance", envVars, new string[] { });
        }
        #endregion
    }

}
