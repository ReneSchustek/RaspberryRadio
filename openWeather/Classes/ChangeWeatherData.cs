using System;
using System.Globalization;

namespace OpenWeather.Classes
{
    public class ChangeWeatherData
    {
        /// <summary>
        /// Erstellt den Text für die Bewölkung
        /// </summary>
        /// <param name="percent">Wolkendichte</param>
        /// <returns>Bewölkungsbezeichnung</returns>
        public static string CreateCloudsDescription(long percent)
        {
            if (percent < 30) { return "sonniger Himmel"; }
            if (percent >= 30 && percent < 70) { return "teilweise Bewölkt"; }
            if (percent > 70) { return "bedeckter Himmel"; }

            return "sonniger Himmel";
        }

        /// <summary>
        /// Korrektur für die Schreibweise der Wetterbeschreibung
        /// </summary>
        /// <param name="weatherDescription">empfangene Beschreibung</param>
        /// <returns>korrigierte Beschreibung</returns>
        public static string CreateWeatherDescription(string weatherDescription)
        {
            weatherDescription = weatherDescription.Replace("himmel", "Himmel");
            weatherDescription = weatherDescription.Replace("regen", "Regen");
            weatherDescription = weatherDescription.Replace("schnee", "Schnee");
            weatherDescription = weatherDescription.Replace("wolken", "Wolken");
            weatherDescription = weatherDescription.Replace("schneeregen", "Schneeregen");

            return weatherDescription;
        }

        /// <summary>
        /// Windrichtung ermitteln
        /// </summary>
        /// <param name="deg">Windrichtung in Grad</param>
        /// <returns>Windrichtung Text</returns>
        public static string CreateWindDeg(double deg)
        {
            if (deg >= 0 && deg < 22.5) { return "Nord"; }
            if (deg >= 22.5 && deg < 67.5) { return "Nord-Ost"; }
            if (deg >= 67.5 && deg < 112.5) { return "Ost"; }
            if (deg >= 112.5 && deg < 157.5) { return "Süd-Ost"; }
            if (deg >= 157.5 && deg < 202.5) { return "Süd"; }
            if (deg >= 202.5 && deg < 247.5) { return "Süd-West"; }
            if (deg >= 247.5 && deg < 292.5) { return "West"; }
            if (deg >= 292.5 && deg < 337.5) { return "Nord-West"; }
            if (deg >= 337.5 && deg <= 360) { return "Nord"; }

            return string.Empty;
        }

        /// <summary>
        /// Abkürzung für den Wochentag ermitteln
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns>Mo, Di, Mi, ...</returns>
        public static string CreateWeekday(DateTime date)
        {
            return date.ToString("ddd", new CultureInfo("de-DE"));
        }
    }
}
