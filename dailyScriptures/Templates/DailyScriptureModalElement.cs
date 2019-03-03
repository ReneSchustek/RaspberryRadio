namespace DailyScriptures.Templates
{
    public class DailyScriptureModalElement
    {
        public static string Get()
        {
            return "<div class=\"modal fade\" id=\"dailyFullText-{{Language}}\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"messageTitle\" aria-hidden=\"true\">" +
                "<div class=\"modal-dialog modal-dialog-centered\" role=\"document\">" +
                    "<div class=\"modal-content\">" +
                        "<div class=\"modal-header\">" +
                            "<p class=\"modal-title\" id=\"dailyFullTextTitle-{{Language}}\">{{Text}}</h6>" +
                        "</div>" +
                        "<div class=\"modal-body\" id=\"dailyFullTextBody-{{Language}}\">{{Comment}}</div>" +
                        "<div class=\"modal-footer\">" +
                            "<button type = \"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">Schlie&szlig;en</button>" +
                        "</div>" +
                    "</div>" +
                "</div>" +
            "</div>";
        }
    }
}
