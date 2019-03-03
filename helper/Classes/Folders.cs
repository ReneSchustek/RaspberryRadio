using System.IO;

namespace Helper.Classes
{
    public static class Folders
    {
        private readonly static string logDirectory = "Log";
        private readonly static string confDirectory = "Conf";
        private readonly static string templateDirectory = "Templates";
        private readonly static string fileDirectory = "Files";

        /// <summary>
        /// Gibt den Pfad für das gewünschte Verzeichnis zurück. Default: Anwendungspfad
        /// </summary>
        /// <param name="request">Das gewünschte Verzeichnis</param>
        /// <returns>Den Pfad für das gewünsche Verzeichnis</returns>
        /// <remarks>
        ///     app -> Anwendungspfad
        ///     log -> LogVerzeichnis
        ///     conf -> Konfigurationsverzeichnis
        ///     conftemplate -> Vorlagenverzeichnis Konfiguration
        ///     database -> Verzeichnis für die Datenbankdatei
        ///     files -> Verzeichnis für Dateien
        ///</remarks>
        public static string Get(string request)
        {
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;

            switch (request.ToLower())
            {
                case "app":
                    return appPath;

                case "log":
                    if (!Directory.Exists(Path.Combine(appPath, logDirectory)))
                        Directory.CreateDirectory(Path.Combine(appPath, logDirectory));

                    return Path.Combine(appPath, logDirectory);

                case "conf":
                    if (!Directory.Exists(Path.Combine(appPath, confDirectory)))
                        Directory.CreateDirectory(Path.Combine(appPath, confDirectory));

                    return Path.Combine(appPath, confDirectory);

                case "conftemplate":
                    if (!Directory.Exists(Path.Combine(appPath, confDirectory, templateDirectory)))
                        Directory.CreateDirectory(Path.Combine(appPath, confDirectory, templateDirectory));

                    return Path.Combine(appPath, confDirectory, templateDirectory);

                case "database":
                    if (!Directory.Exists(Path.Combine(appPath, "db")))
                        Directory.CreateDirectory(Path.Combine(appPath, "db"));
                    return Path.Combine(appPath, "db");

                case "files":
                    if (!Directory.Exists(Path.Combine(appPath, fileDirectory)))
                        Directory.CreateDirectory(Path.Combine(appPath, fileDirectory));
                    return Path.Combine(appPath, fileDirectory);

                default:
                    return appPath;
            }
        }
    }
}
