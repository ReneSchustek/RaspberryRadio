using Helper.Objects;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Helper.Classes
{
    public class Config
    {
        private static string configPath;

        /// <summary>
        /// Liest die Config aus
        /// </summary>
        /// <returns>ConfigModel</returns>
        public ConfigModel Read()
        {
            configPath = Path.Combine(Folders.Get("conf"), Files.Get("conf"));

            //Erstellen
            if (!File.Exists(configPath))
            {
                ConfigModel newConfig = new ConfigModel
                {
                    Mode = "normal"
                };
                bool result = Write(newConfig);

                WriteLog.Write("Konfigurationsdatei erstellt.", "info");
            }

            try
            {
                string content = string.Empty;

                using (StreamReader reader = new StreamReader(configPath))
                {
                    content = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<ConfigModel>(content);
            }
            catch (Exception ex) { throw new Exception("Fehler beim Aktualisieren eines Eintrags der Tabelle WeatherCities: " + ex.ToString()); }

        }

        /// <summary>
        /// Schreibt die Config
        /// </summary>
        /// <param name="config">Die Konfiguration.</param>
        /// <returns>Boolean</returns>
        public bool Write(ConfigModel config)
        {
            configPath = Path.Combine(Folders.Get("conf"), Files.Get("conf"));

            try
            {
                string content = JsonConvert.SerializeObject(config);

                using (FileStream fs = File.Create(configPath))
                {
                    Byte[] contBytes = new UTF8Encoding(true).GetBytes(content);
                    fs.Write(contBytes, 0, contBytes.Length);

                }

                return true;
            }
            catch (Exception ex) { throw new Exception("Fehler beim Aktualisieren eines Eintrags der Tabelle WeatherCities: " + ex.ToString()); }
        }
    }
}
