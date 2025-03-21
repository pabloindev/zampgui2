using System;
using System.Collections.Generic;
using System.IO;

namespace MyApplicazione
{
    public class IniFile
    {
        // Dizionario che contiene le sezioni e i relativi valori (chiave, valore)
        private readonly Dictionary<string, Dictionary<string, string>> _data;

        /// <summary>
        /// Costruttore che accetta il percorso del file INI.
        /// Se il file non esiste viene sollevata un'eccezione.
        /// </summary>
        /// <param name="filePath">Percorso del file INI</param>
        public IniFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Il file .ini non è stato trovato.", filePath);
            }

            _data = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            ParseFile(filePath);
        }

        /// <summary>
        /// Metodo interno che esegue il parsing del file INI.
        /// </summary>
        /// <param name="filePath">Percorso del file INI da leggere</param>
        private void ParseFile(string filePath)
        {
            string currentSection = string.Empty;
            foreach (string line in File.ReadAllLines(filePath))
            {
                // Rimuove spazi iniziali e finali
                string trimmedLine = line.Trim();

                // Salta righe vuote o commenti (assumendo che commenti inizino con ';' o '#')
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // Se la riga rappresenta una sezione (es. [SoftwareVersion])
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();
                    if (!_data.ContainsKey(currentSection))
                    {
                        _data[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    }
                }
                else
                {
                    // La riga dovrebbe essere del tipo chiave=valore
                    int separatorIndex = trimmedLine.IndexOf('=');
                    if (separatorIndex > 0)
                    {
                        string key = trimmedLine.Substring(0, separatorIndex).Trim();
                        string value = trimmedLine.Substring(separatorIndex + 1).Trim();

                        // Se non è stata definita alcuna sezione, si può utilizzare una sezione "default"
                        if (string.IsNullOrEmpty(currentSection))
                        {
                            currentSection = "default";
                            if (!_data.ContainsKey(currentSection))
                            {
                                _data[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            }
                        }

                        _data[currentSection][key] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Restituisce il valore associato a una determinata chiave nella sezione specificata.
        /// Se la sezione o la chiave non esistono, viene restituito null.
        /// </summary>
        /// <param name="section">Il nome della sezione</param>
        /// <param name="key">La chiave da cercare</param>
        /// <returns>Il valore associato oppure null se non trovato</returns>
        public string GetValue(string section, string key)
        {
            if (_data.TryGetValue(section, out var sectionData))
            {
                if (sectionData.TryGetValue(key, out var value))
                {
                    return value;
                }
            }

            return null;
        }
    }
}