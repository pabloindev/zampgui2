using System;
using System.Collections.Generic;
using System.IO;

namespace ZampGUI2.helper
{
    /// <summary>
    /// Classe per gestire la lettura e scrittura di file INI.
    /// I dati vengono organizzati in sezioni (dette anche "categorie")
    /// e ogni sezione contiene coppie chiave-valore.
    /// </summary>
    public class IniFile
    {
        private string filePath;
        // La struttura dati utilizza un dizionario il cui valore è a sua volta un dizionario
        // in cui vengono memorizzate le coppie chiave-valore.
        private Dictionary<string, Dictionary<string, string>> data;


        /// <summary>
        /// Costruttore: riceve il percorso del file INI.
        /// Viene richiamato il metodo Load per caricare i dati dal file, se esiste.
        /// </summary>
        /// <param name="filePath">Percorso completo del file INI</param>
        public IniFile(string filePath)
        {
            this.filePath = filePath;
            data = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            Load();
        }

        /// <summary>
        /// Carica i dati dal file INI nella struttura dati interna.
        /// Viene supportato il formato con commenti (linee che iniziano con ';') e sezioni racchiuse tra [ e ].
        /// Se non viene specificata alcuna sezione per una coppia chiave-valore,
        /// la coppia viene inserita in una sezione "default".
        /// </summary>
        public void Load()
        {
            // Se il file non esiste, non fare nulla
            if (!File.Exists(filePath))
            {
                return;
            }

            string currentSection = "";
            foreach (string line in File.ReadAllLines(filePath))
            {
                string trimmedLine = line.Trim();

                // Ignora linee vuote o commenti
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";"))
                {
                    continue;
                }

                // Se è una sezione, ad es. [Impostazioni]
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();
                    if (!data.ContainsKey(currentSection))
                    {
                        data[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    }
                }
                else
                {
                    // Se si tratta di una coppia chiave=valore
                    int pos = trimmedLine.IndexOf('=');
                    if (pos > 0)
                    {
                        string key = trimmedLine.Substring(0, pos).Trim();
                        string value = trimmedLine.Substring(pos + 1).Trim();

                        // Se non è stata definita una sezione, utilizza "default"
                        if (string.IsNullOrEmpty(currentSection))
                        {
                            currentSection = "default";
                            if (!data.ContainsKey(currentSection))
                            {
                                data[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            }
                        }
                        data[currentSection][key] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Restituisce il valore associato alla chiave all'interno della sezione specificata.
        /// Se la chiave o la sezione non esistono, viene restituito il valore di default passato come parametro.
        /// </summary>
        /// <param name="section">La sezione in cui cercare la chiave</param>
        /// <param name="key">La chiave richiesta</param>
        /// <param name="defaultValue">Valore restituito se la chiave non esiste</param>
        /// <returns>Il valore associato alla chiave oppure il valore di default</returns>
        public string Read(string section, string key, string defaultValue = "")
        {
            if (data.ContainsKey(section) && data[section].ContainsKey(key))
            {
                return data[section][key];
            }
            return defaultValue;
        }

        /// <summary>
        /// Imposta il valore per una data chiave all'interno della sezione specificata.
        /// Se la sezione non esiste, verrà creata.
        /// </summary>
        /// <param name="section">La sezione in cui inserire la coppia</param>
        /// <param name="key">La chiave da impostare</param>
        /// <param name="value">Il valore associato alla chiave</param>
        public void Write(string section, string key, string value)
        {
            if (!data.ContainsKey(section))
            {
                data[section] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            data[section][key] = value;
        }

        /// <summary>
        /// Salva i dati attualmente presenti nella struttura nel file INI.
        /// Il file verrà riscritto con tutte le sezioni e coppie chiave-valore.
        /// </summary>
        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var section in data)
                {
                    writer.WriteLine("[" + section.Key + "]");
                    foreach (var kvp in section.Value)
                    {
                        writer.WriteLine(kvp.Key + "=" + kvp.Value);
                    }
                    writer.WriteLine(); // Riga vuota per separare le sezioni
                }
            }
        }
    }
}
