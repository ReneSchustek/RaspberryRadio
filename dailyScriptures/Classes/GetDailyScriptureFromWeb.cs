using Database.Model;
using Helper.Classes;
using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DailyScriptures.Classes
{
    public class GetDailyScriptureFromWeb
    {
        /// <summary>
        /// Liest den Tagestext von wol.jw.org
        /// </summary>
        /// <param name="language">DailyScripturesLanguageModel</param>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public static async Task<DailyScriptureModel> GetAsync(DailyScriptureLanguageModel language, string date = "")
        {

            string urlPart = string.Empty;
            if (date != string.Empty && date != "")
            {
                DateTime curDate = DateTime.Parse(date);
                urlPart = curDate.Year + "/" + curDate.Month + "/" + curDate.Day;
            }
            else
            {
                DateTime now = DateTime.Now;
                urlPart = now.Year + "/" + now.Month + "/" + now.Day;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = language.Url;

                    //Heutiges Datum auslesen und passende URL zusammenfügen
                    url = url + "/" + DateTime.Now.Year + "/" + (int)DateTime.Now.Month + "/" + (int)DateTime.Now.Day;

                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = await client.GetAsync(url);

                    string result = await response.Content.ReadAsStringAsync();

                    HtmlDocument htmlBody = new HtmlDocument();
                    htmlBody.LoadHtml(result);

                    DailyScriptureModel dailyScripture = new DailyScriptureModel
                    {
                        Language = language.Language
                    };

                    string comment = htmlBody.DocumentNode.SelectSingleNode("(//p[contains(@class, 'sb')])[1]").InnerText;
                    dailyScripture.Comment = RemoveHtml.Remove(comment);

                    string publication = htmlBody.DocumentNode.SelectSingleNode("(//div[contains(@class, 'cardLine1Prominent')])[2]").InnerText;
                    publication = Regex.Replace(publication, @"\t|\n|\r", "");
                    dailyScripture.Publication = RemoveHtml.Remove(publication).Trim();

                    string text = htmlBody.DocumentNode.SelectSingleNode("(//p[contains(@class, 'themeScrp')])[1]").InnerText;
                    dailyScripture.Text = RemoveHtml.Remove(text);

                    string title = htmlBody.DocumentNode.SelectSingleNode("(//div[contains(@class, 'todayItems')]//h2)[1]").InnerText;
                    dailyScripture.Title = RemoveHtml.Remove(title);

                    return dailyScripture;
                }
                catch (HttpRequestException ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return null;
                }
            }
        }
    }
}
