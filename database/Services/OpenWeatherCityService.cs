using Database.Classes;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Services
{
    public class OpenWeatherCityService
    {
        #region Models

        #endregion

        #region Constructor
        public OpenWeatherCityService() { }
        #endregion

        #region Create
        /// <summary>
        /// Speichern eines Datensatzes in der Tabelle OpenWeatherCity
        /// </summary>
        /// <param name="dailyScripture">OpenWeatherCityModel</param>
        /// <returns>Id des gespeicherten Datensatzess</returns>
        public async Task<Int32> CreateAsync(OpenWeatherCityModel city)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (city == null) { return 0; }

            int result = 0;

            //Prüfen, ob Eintrag existiert
            using (var context = db)
            {
                try
                {
                    OpenWeatherCityModel curEntries = await context.OpenWeatherCity.Where(x => x.CityId == city.CityId).AsNoTracking().FirstOrDefaultAsync();
                    if (curEntries != null) { return await UpdateAsync(city, curEntries.Id); }

                    curEntries = await ReadByLongLatAsync(city.Lon, city.Lat);
                    if (curEntries != null) { return await UpdateAsync(city, curEntries.Id); }


                    city.CreatedDate = DateTime.Now;
                    city.UpdatedDate = DateTime.Now;
                    await context.OpenWeatherCity.AddAsync(city);
                    await context.SaveChangesAsync();

                    result = city.Id;
                }
                catch (Exception ex) { throw new Exception("Create OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Read
        /// <summary>
        /// Lesen aller Datensätze der Tabelle OpenWeatherCity 
        /// </summary>
        /// <returns>IList aller Datensätze</returns>
        public async Task<IList<OpenWeatherCityModel>> ReadAllAsync()
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<OpenWeatherCityModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherCity.OrderBy(x => x.Name).AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadAll OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }


        /// <summary>
        /// Liest einen Datensatz nach der Id in der Tabelle OpenWeatherCity aus
        /// </summary>
        /// <param name="id">Id des eintrags</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<OpenWeatherCityModel> ReadByIdAsync(int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            OpenWeatherCityModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherCity.Where(x => x.Id == id).AsNoTracking().SingleOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadById OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suche eines Datensatzes nach der CityId in der Tabelle OpenWeatherCity
        /// </summary>
        /// <param name="title">Titel</param>
        /// <param name="language">Language</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<OpenWeatherCityModel> ReadByCityIdAsync(int cityId)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            //Kein Titel oder Sprache angegeben
            if (cityId == 0) { throw new Exception("Es wurde keine eindeutige Id angegeben."); }

            OpenWeatherCityModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherCity.Where(x => x.CityId == cityId).AsNoTracking().FirstOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadByCityId OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suche eines Datensatzes nach Koordinaten in der Tabelle OpenWeatherCity
        /// </summary>
        /// <param name="lon">Longitude</param>
        /// <param name="lat">Latitude</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<OpenWeatherCityModel> ReadByLongLatAsync(decimal lon, decimal lat)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            OpenWeatherCityModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherCity.Where(x => x.Lon == lon && x.Lat == lat).AsNoTracking().FirstOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadByLongLatAsync OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen der ersten 100 Datensätze nach Stadt und Land in der Tabelle OpenWeatherCity
        /// </summary>
        /// <param name="name">Name der Stadt</param>
        /// <param name="country">Länderkürzel</param>
        /// <returns>Gefundene Datensätze</returns>
        public async Task<IList<OpenWeatherCityModel>> ReadByNameCountryAsync(string name, string country)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<OpenWeatherCityModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherCity.Where(x => x.Name.StartsWith(name) && x.Country == country).OrderBy(x => x.Name).AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadByNameCountry OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen der ersten 100 Datensätze nach Städtename in der Tabelle OpenWeatherCity
        /// </summary>
        /// <param name="name">Name der Stadt</param>
        /// <returns>Gefundene Datensätze</returns>
        public async Task<IList<OpenWeatherCityModel>> ReadByNameAsync(string name)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<OpenWeatherCityModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherCity.Where(x => x.Name.Contains(name)).OrderBy(x => x.Name).AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadByName OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen der Datensätze nach Land in der Tabelle OpenWeatherCity
        /// </summary>
        /// <param name="country">Länderkürzel</param>
        /// <returns>Gefundene Datensätze</returns>
        public async Task<IList<OpenWeatherCityModel>> ReadByCountryAsync(string country)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<OpenWeatherCityModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.OpenWeatherCity.Where(x => x.Country == country).OrderBy(x => x.Name).AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadByCountry OpenWeatherCity: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update eines Datensatzes in der Tabelle OpenWeatherCity
        /// </summary>
        /// <param name="dailyScripture">OpenWeatherCityModel</param>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Id des geänderten Datensatzes</returns>
        public async Task<Int32> UpdateAsync(OpenWeatherCityModel city, int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                OpenWeatherCityModel curEntry = await context.OpenWeatherCity.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        curEntry.CityId = city.CityId;
                        curEntry.Country = city.Country;
                        curEntry.Lat = city.Lat;
                        curEntry.Lon = city.Lon;
                        curEntry.Name = city.Name;
                        curEntry.UpdatedById = city.UpdatedById;
                        curEntry.UpdatedDate = DateTime.Now;

                        context.OpenWeatherCity.Attach(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Update OpenWeatherCity: " + ex.ToString()); }
                }
            }

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Löschung einen Datensatz in der Tabelle OpenWeatherCity
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
                OpenWeatherCityModel curEntry = await context.OpenWeatherCity.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        context.OpenWeatherCity.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Delete OpenWeatherCity: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion
    }
}
