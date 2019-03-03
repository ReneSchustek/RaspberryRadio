namespace Helper.Classes
{
    public static class Files
    {

        private readonly static string errorLogFile = "error.log"; //error
        private readonly static string logFile = "radio.log"; //log
        private readonly static string confFile = "config.dll"; //conf

        /// <summary>
        /// Gibt den Dateinamen zurück
        /// </summary>
        /// <param name="request">Die gewünsche Datei</param>
        /// <returns>Der Dateinames</returns>
        /// <remarks>
        ///     error -> error.log
        ///     log -> kitchenAid.log
        ///     conf -> config.xml
        /// </remarks>
        public static string Get(string request)
        {
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;

            switch (request.ToLower())
            {
                case "error":
                    return errorLogFile;

                case "log":
                    return logFile;

                case "conf":
                    return confFile;

                default:
                    return string.Empty;
            }
        }
    }
}
