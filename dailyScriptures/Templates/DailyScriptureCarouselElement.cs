namespace DailyScriptures.Templates
{
    public class DailyScriptureCarouselElement
    {
        public static string Get()
        {
            return "<div class=\"card bg-dark mb-3 card-dailyScripture h-75 d-inline-block\">" +
                        "<div class=\"card-header card-header-dailyScripture\">" +
                            "<div class=\"row row-eq-height\">" +
                                "<div class=\"col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2\">" +
                                    "<span class=\"far fa-calendar daily-thumb\"></span>" +
                                "</div>" +
                                "<div class=\"col-10 col-sm-10 col-md-10 col-lg-10 col-xl-10 text-left\">" +
                                    "<h6 id=\"daily-title-{{Language}}\">{{Title}}</h6>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"card-body card-body-dailyScripture\">" +
                            "<div class=\"text-center\">" +
                                "<p id=\"daily-text-{{Language}}\" class=\"card-text card-text-dailyScripture\">{{Text}}</p>" +
                            "</div>" +
                            "<p>" +
                                "<div class=\"text-center\">" +
                                    "<button class=\"button-transparent\" data-toggle=\"modal\" data-target=\"#dailyFullText-{{Language}}\">" +
                                        "<span class=\"fa fa-caret-down\" style=\"color: lightgray;\"></span>" +
                                    "</button>" +
                                "</div>" +
                            "</p>" +
                        "</div>" +
                   "</div>";
        }
    }
}
