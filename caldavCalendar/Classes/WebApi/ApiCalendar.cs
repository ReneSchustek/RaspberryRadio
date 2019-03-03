using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calender.Classes.WebApi
{
    [Route("api/calendar")]
    [ApiController]
    public class ApiCalendar : Controller
    {
        #region Models

        #endregion

        #region Constructor
        public ApiCalendar() { }
        #endregion

        /// <summary>
        /// Listet alle gespeicherten Kalender auf
        /// </summary>
        /// <returns>IList<CalendarModel><returns>
        [HttpGet]
        public async Task<IList<CalendarModel>> LoadAsync()
        {
            try
            {
                CalendarService calendarService = new CalendarService();
                return await calendarService.ReadAllAsync();
            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiCalendars Load: " + ex.ToString(), "error");

                return new List<CalendarModel>();
            }
        }

        /// <summary>
        /// Liest eine bestimmte ID aus
        /// </summary>
        /// <returns>CalendarModel</returns>
        [HttpGet("{id}")]
        public async Task<CalendarModel> LoadByIdAsync(int id)
        {
            try
            {
                CalendarService calendarService = new CalendarService();
                return await calendarService.ReadByIdAsync(id);
            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiCalendars LoadById: " + ex.ToString(), "error");

                return new CalendarModel();
            }
        }
    }
}
