using Database.Classes;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Services
{
    public class OpenWeatherSavedCitiesService
    {
        #region Models

        #endregion

        #region Constructor
        public OpenWeatherSavedCitiesService() { }
        #endregion

        #region Create
        /// <summary>
        /// Speichern eines Datensatzes in der Tabelle OpenWeatherSavedCities
        /// </summary>
        /// <param name="savedCity">OpenWeatherSavedCitiesModel</param>
        /// <returns>Id des gespeicherten Datensatzess</returns>
        public async Task<Int32> CreateAsync(OpenWeatherSavedCitiesModel savedCity)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (savedCity == null) { return 0; }

            int result = 0;

            //Prüfen ob Eintrag existiert
            OpenWeatherSavedCitiesModel curEntries = await ReadByWeatherCitiesId(savedCity.WeatherCitiesId);
            if (curEntries != null) { return await UpdateAsync(savedCity, curEntries.Id); }

            using (DatabaseContext context = db)
            {
                try
                {
                    savedCity.CreatedDate = DateTime.Now;
                    savedCity.UpdatedDate = DateTime.Now;
                    await context.OpenWeatherSavedCities.AddAsync(savedCity);
                    await context.SaveChangesAsync();

                    result = savedCity.Id;
                }
                catch (Exception ex) { throw new Exception("Create OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Read
        /// <summary>
        /// Lesen aller Datensätze der Tabelle OpenWeatherSavedCities
        /// </summary>
        /// <returns>IList aller Datensätze</returns>
        public async Task<IList<OpenWeatherSavedCitiesModel>> ReadAllAsync()
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<OpenWeatherSavedCitiesModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherSavedCities.AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadAll OpenWeatherSavedCities: " + ex.ToString()); }

            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen eines Datensatzes nach WeatherCitiesId in der Tabelle OpenWeatherSavedCities
        /// </summary>
        /// <param name="weatherCitiesId">WeatherCitiesId</param>weatherCitiesId
        /// <returns>Gefundener Datensatz</returns>
        public async Task<OpenWeatherSavedCitiesModel> ReadByWeatherCitiesId(int weatherCitiesId)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            //Keine ID angegeben
            if (weatherCitiesId == 0) { throw new Exception("Es wurde keine eindeutige Id angegeben."); }

            OpenWeatherSavedCitiesModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherSavedCities.Where(x => x.WeatherCitiesId == weatherCitiesId).AsNoTracking().FirstOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadByWeatherCitiesId OpenWeatherSavedCities: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update eines Datensatzes in der Tabelle OpenWeatherSavedCities
        /// </summary>
        /// <param name="savedCity">OpenWeatherSavedCitiesModel</param>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Id des geänderten Datensatzes</returns>
        public async Task<Int32> UpdateAsync(OpenWeatherSavedCitiesModel savedCity, int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                OpenWeatherSavedCitiesModel curEntry = await context.OpenWeatherSavedCities.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        curEntry.WeatherCitiesId = savedCity.WeatherCitiesId;
                        curEntry.UpdatedById = savedCity.UpdatedById;
                        curEntry.UpdatedDate = DateTime.Now;

                        context.OpenWeatherSavedCities.Attach(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Update OpenWeatherCity: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Löschung einen Datensatz in der Tabelle OpenWeatherSavedCities
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
                OpenWeatherSavedCitiesModel curEntry = await context.OpenWeatherSavedCities.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        context.OpenWeatherSavedCities.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Delete OpenWeatherSavedCities: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion
    }
}
