using Database.Classes;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Services
{
    public class CalendarService
    {
        #region Models

        #endregion

        #region Constructor
        public CalendarService() { }
        #endregion

        #region Create
        /// <summary>
        /// Asynchrones Speichern eines Datensatzes in der Tabelle Calendar
        /// </summary>
        /// <param name="calendar">CalendarModel</param>
        /// <returns>Id des gespeicherten Datensatzess</returns>
        public async Task<Int32> CreateAsync(CalendarModel calendar)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (calendar == null) { return 0; }

            int result = 0;

            //Prüfen, ob Eintrag existiert
            IList<CalendarModel> curEntries = await ReadAllAsync();
            if (curEntries.Count > 0) { return await UpdateAsync(calendar, 1); }

            using (DatabaseContext context = db)
            {
                try
                {
                    calendar.CreatedDate = DateTime.Now;
                    calendar.UpdatedDate = DateTime.Now;
                    await context.Calendars.AddAsync(calendar);
                    await context.SaveChangesAsync();

                    result = calendar.Id;
                }
                catch (Exception ex) { throw new Exception("Create Calendar: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Read
        /// <summary>
        /// Asynchrones Lesen aller Datensätze der Tabelle Calendar
        /// </summary>
        /// <returns>IList aller Datensätze</returns>
        public async Task<IList<CalendarModel>> ReadAllAsync()
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<CalendarModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.Calendars.AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadAll Calendar: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Asynchrones Suchen eines Datensatzes nach ID in der Tabelle Calendar
        /// </summary>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<CalendarModel> ReadByIdAsync(int id)
        {
            if (id == 0) { throw new Exception("Es wurde eine fehlerhafte Id angegeben."); }

            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            CalendarModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.Calendars.FindAsync(id); }
                catch (Exception ex) { throw new Exception("ReadById Calendar: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Asynchrones Update eines Datensatzes in der Tabelle Calendar
        /// </summary>
        /// <param name="Calendar">CalendarModel</param>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Id des geänderten Datensatzes</returns>
        public async Task<Int32> UpdateAsync(CalendarModel Calendar, int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                CalendarModel curEntry = await context.Calendars.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        curEntry.Name = Calendar.Name;
                        curEntry.Password = Calendar.Password;
                        curEntry.Username = Calendar.Username;
                        curEntry.Url = Calendar.Url;
                        curEntry.UpdatedById = Calendar.UpdatedById;
                        curEntry.UpdatedDate = DateTime.Now;

                        context.Calendars.Attach(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Update Calendar: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Asynchrone Löschung einen Datensatz in der Tabelle Calendar
        /// </summary>
        /// <param name="id">Id des Datenbankeintrags</param>
        /// <returns>Id des gelöschten Datensatzes</returns>
        public async Task<Int32> DeleteAsync(int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                CalendarModel curEntry = await context.Calendars.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        context.Calendars.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Delete Calendar: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion
    }
}
