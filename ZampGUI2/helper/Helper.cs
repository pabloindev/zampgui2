using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Runtime.ConstrainedExecution;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;

namespace ZampGUI2.helper
{
    public class Helper
    {
        IniFile config;
        Percorsi percorsi;
        FormMain form;

        public Helper(IniFile config, FormMain form)
        {
            this.config = config;
            this.percorsi = new Percorsi(config);
            this.form = form;
        }

        // conotrollo se esiste nuovo aggiornamento online
        public (bool, string) checkUpdateSoftware()
        {
            string results = null;
            string urlUpdate = config.Read("ImpostazioniGenerali", "urlUpdate");
            string currentVersion = config.Read("ImpostazioniGenerali", "currentVersion");
            string uuid = config.Read("ImpostazioniGenerali", "uuid");

            if(config.Read("ImpostazioniGenerali", "checkLastVersionApp") == "true")
            {
                try
                {
                    NameValueCollection values = new NameValueCollection();
                    values.Add("reqinfo", uuid);
                    values.Add("ver", currentVersion);

                    using (var wc = new System.Net.WebClient())
                    {
                        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        byte[] result = wc.UploadValues(urlUpdate, "POST", values);
                        results = System.Text.Encoding.UTF8.GetString(result);
                    }
                    if (string.IsNullOrEmpty(results))
                    {
                        results = "{}";
                    }
                    JObject jobj = JObject.Parse(results);
                    string url_latest_vers = jobj.Value<string>("url_latest_vers");
                    string ver_web = jobj.Value<string>("ver");

                    return (!currentVersion.Equals(ver_web), url_latest_vers);
                }
                catch (Exception e)
                {

                }
            }
            return (false, "");
        }
        public void mariadb_aggiorna_myini(string mariadb_inipath, string mariadb_datadir, string mariadb_plugindir)
        {
            // Leggi tutte le righe del file
            mariadb_datadir = mariadb_datadir.Replace("\\", "/");
            mariadb_plugindir = mariadb_plugindir.Replace("\\", "/");

            string[] lines = File.ReadAllLines(mariadb_inipath);
            string[] newLines = new string[lines.Length];

            // Processa ogni riga
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                // Sostituisci datadir
                if (line.StartsWith("datadir="))
                {
                    newLines[i] = $"datadir={mariadb_datadir}";
                }
                // Sostituisci plugin-dir
                else if (line.StartsWith("plugin-dir="))
                {
                    newLines[i] = $"plugin-dir={mariadb_plugindir}";
                }
                // Mantieni tutte le altre righe invariate
                else
                {
                    newLines[i] = line;
                }
            }

            // Scrivi le modifiche nel file
            File.WriteAllLines(mariadb_inipath, newLines);
        }
        public void MostraDialogoConTestoELink(string text, string linkText, string linkUrl)
        {
            // Crea una form dinamica
            Form dialogForm = new Form()
            {
                Text = "Information",
                Size = new Size(400, 200),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(30, 30, 30), // Sfondo scuro
                ForeColor = Color.White, // Testo bianco
            };
            

            // Aggiungi un Label per il testo
            Label label = new Label()
            {
                Text = text,
                Location = new Point(20, 20),
                AutoSize = true,
                BackColor = Color.FromArgb(30, 30, 30), // Sfondo scuro
                ForeColor = Color.White // Testo bianco
            };

            // Aggiungi un LinkLabel per il link cliccabile
            LinkLabel linkLabel = new LinkLabel()
            {
                Text = linkText,
                Location = new Point(20, 50),
                AutoSize = true,
                LinkColor = Color.LightBlue, // Colore del link
                ActiveLinkColor = Color.Cyan, // Colore del link attivo
                VisitedLinkColor = Color.LightBlue, // Colore del link visitato
                Tag = linkUrl // Usiamo Tag per memorizzare l'URL
            };

            // Gestisci il click sul link
            linkLabel.LinkClicked += (sender, e) =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = linkUrl,
                    UseShellExecute = true
                });
            };

            // Aggiungi i controlli alla form
            dialogForm.Controls.Add(label);
            dialogForm.Controls.Add(linkLabel);

            // Mostra la form come dialog
            dialogForm.ShowDialog();
        }

        public void openFileWithEditor(string editorPath, string filePath, bool runAsAdmin = false)
        {
            try
            {
                // Verifica se il file da aprire esiste
                if (!System.IO.File.Exists(filePath))
                {
                    MessageBox.Show("file " + filePath + " does not exist");
                    return;
                }

                // Se runAsAdmin è true, usa notepad.exe come editor
                if (runAsAdmin)
                {
                    editorPath = "notepad.exe";
                }

                // Verifica se l'editor è un eseguibile di sistema (es. "notepad.exe" o "notepad")
                bool isSystemEditor = editorPath.Equals("notepad.exe", StringComparison.OrdinalIgnoreCase) ||
                                      editorPath.Equals("notepad", StringComparison.OrdinalIgnoreCase);

                // Se non è un editor di sistema, verifica che il percorso dell'editor esista
                if (!isSystemEditor && !System.IO.File.Exists(editorPath))
                {
                    MessageBox.Show("the current editor " + editorPath + " does not exist");
                    return;
                }

                // Configura ProcessStartInfo
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = editorPath,
                    Arguments = $"\"{filePath}\"", // Passa il file come argomento
                    UseShellExecute = true, // Usa ShellExecute per avviare con privilegi di amministratore
                    Verb = runAsAdmin ? "runas" : "" // Imposta "runas" per avviare come amministratore
                };

                // Avvia il processo
                Process.Start(startInfo);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Editor or file not found.");
            }
            catch (Win32Exception)
            {
                MessageBox.Show("Error while running the editor");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while opening the file \"" + filePath + $"\": {ex.Message}");
            }
        }

        #region gestione processi
        public bool runZampGUI_Console(string typeOfJob, Dictionary<string, string> environmentVariables, string[] args)
        {
            string consoleAppPath = percorsi.zampgui_console_exe;
            string combinedPaths = string.Join(" ", args.Select(p => $"\"{p}\""));

            if (!File.Exists(consoleAppPath))
            {
                MessageBox.Show($"Unable to find ZampGUI_Console.exe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Configurazione del ProcessStartInfo per avviare il processo figlio
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = consoleAppPath,
                Arguments = typeOfJob + (string.IsNullOrEmpty(combinedPaths) ? "" : " " + combinedPaths),
                UseShellExecute = false,  // Consente di impostare le variabili solo per il processo figlio
                CreateNoWindow = false,   // La console verrà mostrata
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = percorsi.zampgui_console_path
            };

            // Impostazione delle variabili d'ambiente specifiche per il processo figlio:
            foreach (var kvp in environmentVariables)
            {
                psi.EnvironmentVariables[kvp.Key] = kvp.Value;
            }
        
            try
            {
                // Avvio del processo console
                using (Process process = Process.Start(psi))
                {
                    // Attende la fine del processo
                    process.WaitForExit();

                    // Recupera il codice di uscita
                    int exitCode = process.ExitCode;

                    if (exitCode == 0)
                    {
                        // OK - Notifica il risultato all'utente
                        //MessageBox.Show($"L'applicazione console ha terminato con exit code: {exitCode}", "Risultato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        // errore
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        public bool IsPortAvailable(int port)
        {
            bool isAvailable = true;

            try
            {
                // Prova a creare un TcpListener sulla porta specificata
                TcpListener tcpListener = new TcpListener(System.Net.IPAddress.Loopback, port);
                tcpListener.Start();
                tcpListener.Stop();
            }
            catch (SocketException)
            {
                // Se viene sollevata un'eccezione, la porta è occupata
                isAvailable = false;
            }

            return isAvailable;
        }
        public bool processo_in_esecuzione(string nome_processo)
        {
            try
            {
                // Rimuovi l'estensione .exe se presente, perché Process.GetProcessesByName la ignora
                string processName = nome_processo.EndsWith(".exe")
                    ? nome_processo.Substring(0, nome_processo.Length - 4)
                    : nome_processo;

                // Ottiene tutti i processi con il nome specificato
                Process[] processi = Process.GetProcessesByName(processName);

                // Se ci sono processi con quel nome, ritorna true
                return processi.Length > 0;
            }
            catch (Exception ex)
            {
                // In caso di errore (es. permessi), ritorna false e logga l'errore se necessario
                // Puoi personalizzare questa parte per gestire l'eccezione come preferisci
                //System.Console.WriteLine($"Errore durante la verifica del processo {nomeProcesso}: {ex.Message}");
                return false;
            }
        }
        public async void processo_start(string nome_processo)
        {
            var envVars = new Dictionary<string, string>();
            string pathprocesso;
            switch (nome_processo)
            {
                case "mariadbd":
                    // aggiorno il file ini prima di lanciare mariadb
                    mariadb_aggiorna_myini(percorsi.mariadb_ini, percorsi.mariadb_datadir, percorsi.mariadb_plugindir);
                    
                    // definisco le variabili di ambiente
                    envVars = new Dictionary<string, string>
                    {
                        { "PATH", percorsi.mariadb_bin + ";" + Environment.GetEnvironmentVariable("PATH") },
                        { "ZAMPGUI_PATH", percorsi.pathBase }
                    };
                    pathprocesso = Path.Combine(percorsi.mariadb_bin, nome_processo + ".exe");

                    await LaunchServerProcessAsync(pathprocesso, percorsi.mariadb_bin, envVars, msg => {
                        try
                        {
                            this.form.textBoxOuput.AppendText(msg + Environment.NewLine);
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    });
                    break;
                case "httpd":
                    // definisco le variabili di ambiente
                    envVars = new Dictionary<string, string>
                    {
                        { "PATH", percorsi.apache_bin + ";" + Environment.GetEnvironmentVariable("PATH") },
                        { "ZAMPGUI_PATH", percorsi.pathBase },
                        { "PHPIniDir", percorsi.php_path },
                        { "CURRENT_VERS", config.Read("FolderName", "currentvers") },
                        { "HTTP_PORT", config.Read("Porte", "httpPort") }
                    };
                    pathprocesso = Path.Combine(percorsi.apache_bin, nome_processo + ".exe");

                    await LaunchServerProcessAsync(pathprocesso, percorsi.apache_bin, envVars, msg => {
                        try
                        {
                            this.form.textBoxOuput.AppendText(msg + Environment.NewLine);
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                    break;
            }
        }
        public (bool Success, string Message) processo_stop(string nomeProcesso)
        {
            try
            {
                string processName = nomeProcesso.EndsWith(".exe")
                    ? nomeProcesso.Substring(0, nomeProcesso.Length - 4)
                    : nomeProcesso;

                Process[] processi = Process.GetProcessesByName(processName);
                if (processi.Length == 0)
                {
                    return (true, $"No {nomeProcesso} process found. No action needed.");
                }

                bool allStopped = true;
                string message = "";
                int processCount = processi.Length;

                foreach (Process processo in processi)
                {
                    try
                    {
                        if (!processo.HasExited)
                        {
                            if (processo.CloseMainWindow())
                            {
                                processo.WaitForExit(5000);
                            }

                            if (!processo.HasExited)
                            {
                                processo.Kill();
                                processo.WaitForExit(2000);
                                message += $"Process {nomeProcesso} (PID: {processo.Id}) forcibly killed." + Environment.NewLine;
                            }
                            else
                            {
                                message += $"Process {nomeProcesso} (PID: {processo.Id}) stopped successfully." + Environment.NewLine;
                            }
                        }
                        else
                        {
                            message += $"Process {nomeProcesso} (PID: {processo.Id}) already terminated." + Environment.NewLine;
                        }
                    }
                    catch (Exception ex)
                    {
                        allStopped = false;
                        message += $"Error stopping {nomeProcesso} (PID: {processo.Id}): {ex.Message}" + Environment.NewLine;
                    }
                    finally
                    {
                        processo.Dispose();
                    }
                }

                if (allStopped)
                {
                    return (true, $"All {nomeProcesso} processes ({processCount}) stopped successfully.\n{message.TrimEnd()}" + Environment.NewLine);
                }
                else
                {
                    return (false, $"Some {nomeProcesso} processes could not be stopped.\n{message.TrimEnd()}" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                return (false, $"General error stopping {nomeProcesso}: {ex.Message}" + Environment.NewLine);
            }
        }
        public void OpenConsole(string startingDirectory, Dictionary<string, string> environmentVariables, string message, string batFilePath = null, List<string> batArguments = null)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = startingDirectory,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            // Divide il messaggio in righe separate
            string[] lines = message.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string echoCommands = string.Join(" & echo ", lines); // Unisce le righe con " & echo "

            // Costruisci il comando da eseguire nella console
            string command = $@"/K ""echo {echoCommands} & cd""";

            // Se il file BAT è specificato e esiste, aggiungi l'esecuzione del BAT al comando
            if (!string.IsNullOrEmpty(batFilePath) && File.Exists(batFilePath))
            {
                // Costruisci la stringa degli argomenti per il BAT
                string arguments = batArguments != null ? string.Join(" ", batArguments.Select(arg => $"\"{arg}\"")) : string.Empty;
                command += $" & \"{batFilePath}\" {arguments}";
            }

            processStartInfo.Arguments = command;

            // Applica le variabili d'ambiente
            foreach (var kvp in environmentVariables)
            {
                processStartInfo.EnvironmentVariables[kvp.Key] = kvp.Value;
            }

            try
            {
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore apertura console: {ex.Message}");
            }
        }
        #endregion

        #region private

        public async Task LaunchServerProcessAsync(string pathprocesso, string workingdir, IDictionary<string, string> environmentVariables, Action<string> outputCallback)
        {
            try
            {
                // Determina il nome eseguibile in base al parametro fornito.
                string exeName = Path.GetFileName(pathprocesso);
                //if (pathprocesso.EndsWith("httpd.exe"))
                //{
                //    exeName = "httpd.exe";
                //}
                //else if (pathprocesso.EndsWith("mariadbd.exe"))
                //{
                //    exeName = "mariadbd.exe";
                //}
                //else
                //{
                //    outputCallback?.Invoke($"Errore: process '{pathprocesso}' unsupported.");
                //    return;
                //}

                // Configura il ProcessStartInfo, impostando UseShellExecute a false per consentire
                // la redirezione degli stream e la modifica delle variabili d'ambiente.
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pathprocesso,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = workingdir
                };

                // Imposta le variabili d'ambiente se fornite.
                if (environmentVariables != null)
                {
                    foreach (var variable in environmentVariables)
                    {
                        psi.Environment[variable.Key] = variable.Value;
                    }
                }

                using (Process process = new Process { StartInfo = psi, EnableRaisingEvents = true })
                {
                    // Registra gli handler per la cattura dell'output e degli errori.
                    process.OutputDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            outputCallback?.Invoke($"[{exeName}] {args.Data}");
                        }
                    };

                    process.ErrorDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            outputCallback?.Invoke($"[{exeName} ERROR] {args.Data}");
                        }
                    };

                    // Avvia il processo.
                    bool started = process.Start();

                    if (!started)
                    {
                        outputCallback?.Invoke($"Errore: impossibile avviare {exeName}.");
                        return;
                    }

                    // Inizia la lettura asincrona degli stream di output ed errori.
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Attendi alcuni secondi per dare il tempo al processo di inizializzarsi.
                    await Task.Delay(3000);

                    // Se il processo non è terminato, lo consideriamo in esecuzione.
                    if (!process.HasExited)
                    {
                        outputCallback?.Invoke($"process {exeName} running");
                    }
                    else
                    {
                        outputCallback?.Invoke($"process {exeName} down with exit code {process.ExitCode}.");
                    }
                }
            }
            catch (Exception ex)
            {
                outputCallback?.Invoke("Error while starting a process: " + ex.Message);
            }
        }
        public void LaunchBatchFileVisible(string batFilePath, string workingdir, IDictionary<string, string> environmentVariables)
        {
            if (!File.Exists(batFilePath))
                throw new FileNotFoundException($"The file {batFilePath} does not exist.");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                // Avvia direttamente il BAT.
                FileName = batFilePath,
                //Arguments = arguments,
                // Utilizziamo il sistema shell in modo che il BAT venga eseguito come da associazione
                UseShellExecute = false,
                CreateNoWindow = false, // Mostra la finestra della console
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = workingdir
            };

            if (environmentVariables != null)
            {
                foreach (var variable in environmentVariables)
                {
                    psi.Environment[variable.Key] = variable.Value;
                }
            }

            Process.Start(psi);
        }
        
        #endregion
    }
}
