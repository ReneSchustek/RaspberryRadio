using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeather.Templates
{
    public class OpenWeatherModalElement
    {
        public static string Get()
        {
            return "<div class=\"modal fade\" id=\"currentWeatherInfo-{{WeatherCityName}}\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"messageTitle\" aria-hidden=\"true\">" +
                "<div class=\"modal-dialog modal-dialog-centered\" role=\"document\">" +
                    "<div class=\"modal-content\">" +
                        "<div class=\"modal-header\">" +
                            "<p class=\"modal-title\" id=\"currentWeatherTitle-{{WeatherCityName}}\">{{WeatherCityName}}</h6>" +
                        "</div>" +
                        "<div class=\"modal-body\" id=\"currentWeatherTextBody-{{WeatherCityName}}\">" +
                            "<div class=\"row\">" +
                                "<div class=\"col-2 col-sm-2 col-md-2 col-lg-2\"> " +
                                    "<span class=\"fa fa-cloud\"></span>" +
                                "</div>" +
                                "<div class=\"col-5 col-sm-5 col-md-5 col-lg-5\"> " +
                                    "<label id=\"currentWeatherCloudDescription-{{WeatherCityName}}\">{{WeatherCloudinessDescription}}</label>" +
                                "</div>" +
                                "<div class=\"col-5 col-sm-5 col-md-5 col-lg-5\"> " +
                                    "<label id=\"currentWeatherCloudPercent-{{WeatherCityName}}\">{{WeatherCloudinessPercent}}</label>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-2 col-sm-2 col-md-2 col-lg-2\"> " +
                                    "<span class=\"fa fa-sun\"></span>" +
                                "</div>" +
                                "<div class=\"col-5 col-sm-5 col-md-5 col-lg-5\"> " +
                                    "<span class=\"fa fa-arrow-up\"></span>&nbsp;<label id=\"currentWeatherSunrise-{{WeatherCityName}}\">{{WeatherSunrise}}</label>" +
                                "</div>" +
                                "<div class=\"col-5 col-sm-5 col-md-5 col-lg-5\"> " +
                                    "<span class=\"fa fa-arrow-down\"></span>&nbsp;<label id=\"currentWeatherSunset-{{WeatherCityName}}\">{{WeatherSunset}}</label>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-2 col-sm-2 col-md-2 col-lg-2\"> " +
                                    "<label style=\"font-weight: bold\">hPa</label>" +
                                "</div>" +
                                "<div class=\"col-10 col-sm-10 col-md-10 col-lg-10\">" +
                                    "<label id=\"currentWeatherPressure-{{WeatherCityName}}\">{{WeatherPressure}}</label>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-2 col-sm-2 col-md-2 col-lg-2\"> " +
                                    "<span class=\"fa fa-tint\"></span>" +
                                "</div>" +
                                "<div class=\"col-10 col-sm-10 col-md-10 col-lg-10\"> " +
                                    "<label id=\"currentWeatherHumidity-{{WeatherCityName}}\">{{WeatherHumidity}}%</label>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-2 col-sm-2 col-md-2 col-lg-2\"> " +
                                    "<span class=\"fa fa-wind\"></span>" +
                                "</div>" +
                                "<div class=\"col-5 col-sm-5 col-md-5 col-lg-5\"> " +
                                    "<label id=\"currentWeatherSpeed-{{WeatherCityName}}\">{{WeatherSpeed}} km/h</label>" +
                                "</div>" +
                                "<div class=\"col-5 col-sm-5 col-md-5 col-lg-5\"> " +
                                    "<span class=\"fa fa-compass\"></span>&nbsp;<label id=\"currentWeatherDeg-{{WeatherCityName}}\">{{WeatherDeg}}</label>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-2 col-sm-2 col-md-2 col-lg-2\"> " +
                                    "<span class=\"fa fa-umbrella-beach\"></span>" +
                                "</div>" +
                                "<div class=\"col-10 col-sm-10 col-md-10 col-lg-10\"> " +
                                    "<label id=\"currentWeatherUv-{{WeatherCityName}}\">{{WeatherUv}} UVI</label>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"modal-footer\">" +
                            "<button type = \"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">Schlie&szlig;en</button>" +
                        "</div>" +
                    "</div>" +
                "</div>" +
            "</div>";
        }
    }
}
