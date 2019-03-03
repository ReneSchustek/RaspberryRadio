namespace OpenWeather.Templates
{
    public class OpenWeatherForecastElement
    {
        /// <summary>
        /// Gibt den Anfangs-Code für die Wettervorhersage pro Stadt zurück
        /// </summary>
        /// <returns>Html-Code</returns>
        public static string GetCityStartElement()
        {
            return "<li>" +
                        "<div class=\"row\">";

        }

        /// <summary>
        /// Gibt den End-Code für die Wettervorhersage pro Stadt zurück
        /// </summary>
        /// <returns>Html-Code</returns>
        public static string GetCityEndElement()
        {
            return "</div>" +
               "</li>";
        }

        /// <summary>
        /// Gibt den Code für die Wettervorhersage pro Vorhersage zurück
        /// </summary>
        /// <returns>Html-Code</returns>
        public static string GetForecastElement()
        {
            return "<div class=\"col-3 col-sm-3 col-md-3 col-lg-3 col-xl-3 mx-auto {{ForecastClass}}\">" +
                        "<div class=\"row\">" +
                            "<div class=\"col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 text-left weather-forecast-small mx-auto\">" +
                                "<div class=\"text-center\">{{ForecastCity}} - {{ForecastTime}}</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"row\">" +
                            "<div class=\"col-6 col-sm-6 col-md-6 col-lg-6 col-xl-6 d-flex align-items-center justify-content-center\">" +
                                "<div class=\"text-right\">" +
                                    "<img src=\"https://openweathermap.org/img/w/{{ForecastImage}}.png\" alt=\"{{ForecastDescription}}\" class=\"weather-forecast-image\" />" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"col-6 col-sm-6 col-md-6 col-lg-6 col-xl-6 d-flex align-items-center justify-content-center\">" +
                                "<div class=\"text-left weather-forecast-temprature\">{{ForecastTemp}}&deg;C</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"row\">" +
                            "<div class=\"col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 text-left weather-forecast-small mx-auto\">" +
                                "<div class=\"text-center\">{{ForecastDescription}}</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>";
        }
    }
}
