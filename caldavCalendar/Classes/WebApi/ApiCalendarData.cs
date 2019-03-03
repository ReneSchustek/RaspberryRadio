using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CaldavCalendar.Classes.WebApi
{
    [Route("api/loadCalendar")]
    [ApiController]
    public class ApiCalendarData : Controller
    {
        #region Model
        #endregion

        #region Constructor
        public ApiCalendarData() { }
        #endregion

        [HttpGet]
        public async Task<string> LoadAsync()
        {
            try
            {
                GetCalendarEvents getCalendarEvents = new GetCalendarEvents();
                return await getCalendarEvents.GetEvents();
            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiCalendarsData Load: " + ex.ToString(), "error");
                return "Error: " + ex.ToString();
            }
        }

    }
}
