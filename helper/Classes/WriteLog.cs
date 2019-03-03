using System;
using System.IO;
using System.Text;

namespace Helper.Classes
{
    public static class WriteLog
    {
        private static string logPath;

        /// <summary>
        /// Schreibt in die LogDatei
        /// </summary>
        /// <param name="message">Die zu schreibende Nachricht</param>
        /// <param name="type">error -> Fehler, info -> Information, warn -> Warnung</param>
        public static void Write(string message, string type = "")
        {
            logPath = Path.Combine(Folders.Get("log"), Files.Get("log"));

            //Nicht über 2MB groß werden
            LimitLogFile(logPath);

            string text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": ";

            switch (type.ToLower())
            {
                case "error":
                    File.AppendAllText(logPath, text + "ERROR - " + message + Environment.NewLine);
                    break;

                case "info":
                    File.AppendAllText(logPath, text + "INFO - " + message + Environment.NewLine);
                    break;

                case "debug":
                    File.AppendAllText(logPath, text + "DEBUG - " + message + Environment.NewLine);
                    break;

                default:
                    File.AppendAllText(logPath, text + "DETAILS: - " + message + Environment.NewLine);
                    break;
            }
        }

        /// <summary>
        /// Hält die Log-Datei unter 2MB
        /// </summary>
        /// <param name="path"></param>
        private static void LimitLogFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            const Int32 BufferSize = 128;
            int counter = 0;
            string newContent = string.Empty;

            while (fileInfo.Length > (2 * 1024 * 1024))
            {
                using (FileStream fileStream = File.OpenRead(path))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            counter++;
                            if (counter < 10) { continue; }
                            newContent += line;
                        }
                    }
                }

                File.WriteAllText(path, newContent);

                fileInfo = new FileInfo(logPath);
            }
        }
    }
}
