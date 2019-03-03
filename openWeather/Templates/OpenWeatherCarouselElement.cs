namespace OpenWeather.Templates
{
    /// <summary>
    /// Template für die Wetteranzeige
    /// </summary>
    public class OpenWeatherCarouselElement
    {
        public static string Get()
        {
            return "<div onclick=\"showCurrentWeatherModal('{{WeatherCityName}}')\">" +
                       "<div class=\"row\"> " +
                            "<div class=\"col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 mx-auto\"> " +
                                 "<div id=\"current-title-{{WeatherCityName}}\" class=\"weather header text-left\">{{WeatherCityName}}</div> " +
                            "</div> " +
                       "</div>" +
                       "<div class=\"row\">" +
                            "<div class=\"col-5 col-sm-5 col-md-5 col-lg-5 col-xl-5 d-flex align-items-center justify-content-center\">" +
                                 "<div>" +
                                      "<img id=\"current-image-{{WeatherCityName}}\" src=\"https://openweathermap.org/img/w/{{WeatherImage}}.png\" alt=\"{{WeatherDescription}}\" class=\"weather-image\" />" +
                                  "</div>" +
                            "</div>" +
                            "<div class=\"col-7 col-sm-7 col-md-7 col-lg-7 col-xl-7 d-flex align-items-center justify-content-center\">" +
                                 "<div id=\"current-temp-{{WeatherCityName}}\" class=\"weather-temprature\">{{WeatherTemprature}}&deg;C</div>" +
                            "</div>" +
                       "</div>" +
                       "<div class=\"row text-center\">" +
                            "<div class=\"col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12\">" +
                                 "<div id=\"current-description-{{WeatherCityName}}\" class=\"weather-small text-center\">{{WeatherDescription}}</div>" +
                            "</div>" +
                       "</div>" +
                   "</div>";
        }
    }
}
