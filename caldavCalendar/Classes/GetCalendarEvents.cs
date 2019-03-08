using Database.Model;
using Database.Services;
using Helper.Classes;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CaldavCalendar.Classes
{
    public class GetCalendarEvents
    {
        /// <summary>
        /// Liest aus einer .ics die Termine für die nächsten 5 Tage
        /// </summary>
        /// <returns>String mit den Terminen</returns>
        public async Task<string> GetEvents()
        {
            try
            {
                string result = string.Empty;

                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("de-DE");

                DateTime searchStart = DateTime.Now;
                DateTime searchEnd = DateTime.Parse(DateTime.Now.AddDays(5).ToShortDateString() + " 0:00");

                CalendarService calendarService = new CalendarService();
                IList<CalendarModel> savedCalendars = await calendarService.ReadAllAsync();

                CalendarCollection calendars = new CalendarCollection();

                foreach (CalendarModel savedCalendar in savedCalendars)
                {
                    Ical.Net.Calendar calendar = await LoadFromUriAsync(new Uri(savedCalendar.Url));
                    calendars.Add(calendar);
                }

                SortedDictionary<long, string> eventDictionary = new SortedDictionary<long, string>();

                foreach (Ical.Net.Calendar calendar in calendars)
                {
                    var occurrences = calendar.GetOccurrences(searchStart, searchEnd);

                    foreach (Occurrence item in occurrences)
                    {
                        CalendarEvent sourceEvent = item.Source as CalendarEvent;

                        DateTime eventBegin = new DateTime(sourceEvent.Start.Ticks);
                        string shortWeekday = culture.DateTimeFormat.GetShortestDayName(eventBegin.DayOfWeek);

                        string elementText = "<span class=\"calendar-content\">" + shortWeekday + ", " + eventBegin.ToShortDateString() + " " + eventBegin.ToShortTimeString() + " - " + sourceEvent.Summary + "</span>";

                        eventDictionary.Add(sourceEvent.Start.Ticks, elementText);
                    }
                }


                //Liste erstellen
                int counter = 1;
                foreach (string value in eventDictionary.Values)
                {
                    if (counter == 1) { result += "<li><span class=\"calendar-content-start\">Anstehende Termine: </span>" + value + "</li>"; }
                    else { result += "<li>" + value + "</li>"; }

                    counter++;
                }


                if (result == string.Empty)
                {
                    result += "<li><span class=\"calendar-content\">Keine anstehenden Termine.</span></li>";
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                WriteLog.Write("GetEvents: " + ex.ToString(), "error");

                return "<span class=\"calendar-content\">Fehler bei der Terminabfrage.</span>";
            }
        }

        /// <summary>
        /// Lädt die .ics vom Link
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<Ical.Net.Calendar> LoadFromUriAsync(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();

                    return Ical.Net.Calendar.Load(result);
                }
            }
        }
    }
}
