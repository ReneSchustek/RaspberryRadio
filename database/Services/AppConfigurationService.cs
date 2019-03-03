using Database.Classes;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Services
{
    public class AppConfigurationService
    {
        #region Models

        #endregion

        #region Constructor
        public AppConfigurationService() { }
        #endregion

        #region Create
        /// <summary>
        /// Speichern eines Datensatzes in der Tabelle Configuration
        /// </summary>
        /// <param name="configuration">ConfigurationModel</param>
        /// <returns>Id des gespeicherten Datensatzess</returns>
        public async Task<Int32> CreateAsync(AppConfigurationModel configuration)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (configuration == null) { return 0; }

            int resultId = 0;

            //Prüfen, ob Eintrag existiert
            AppConfigurationModel configurationTest = await ReadByClientAsync(configuration.Client);

            if (configurationTest != null) { return await UpdateAsync(configuration, 1); }

            //Neuen Eintrag anlegen
            using (DatabaseContext context = db)
            {
                try
                {
                    configuration.CreatedDate = DateTime.Now;
                    configuration.UpdatedDate = DateTime.Now;

                    await context.AppConfiguration.AddAsync(configuration);
                    await context.SaveChangesAsync();

                    resultId = configuration.Id;

                }
                catch (Exception ex) { throw new Exception("Create Configuration: " + ex.ToString()); }
            }

            db.Dispose();

            return resultId;
        }
        #endregion

        #region Read
        /// <summary>
        /// Lesen aller Datensätze der Tabelle Configuration 
        /// </summary>
        /// <returns>IList aller Datensätze</returns>
        public async Task<IList<AppConfigurationModel>> ReadAllAsync()
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<AppConfigurationModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.AppConfiguration.AsNoTracking().OrderBy(x => x.Id).ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadAll Configuration: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen eines Datensatzes nach ID in der Tabelle Configuration
        /// </summary>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<AppConfigurationModel> ReadByIdAsync(int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            AppConfigurationModel result = null;
            using (DatabaseContext context = db)
            {
                try { result = await context.AppConfiguration.FindAsync(id); }
                catch (Exception ex) { throw new Exception("ReadById Configuration: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen eines Datensatzes nach Client in der Tabelle Configuration
        /// </summary>
        /// <param name="client">Client</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<AppConfigurationModel> ReadByClientAsync(string client)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            AppConfigurationModel result = null;
            using (DatabaseContext context = db)
            {
                try { result = await context.AppConfiguration.Where(x => x.Client == client).FirstOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadByClient Configuration: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update eines Datensatzes in der Tabelle Configuration
        /// </summary>
        /// <param name="configuration">ConfigurationModel</param>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Id des geänderten Datensatzes</returns>
        public async Task<Int32> UpdateAsync(AppConfigurationModel configuration, int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                AppConfigurationModel curEntry = await context.AppConfiguration.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {

                    try
                    {
                        curEntry.Client = configuration.Client;
                        curEntry.Lang = configuration.Lang;
                        curEntry.UpdatedById = configuration.UpdatedById;
                        curEntry.UpdatedDate = DateTime.Now;

                        context.AppConfiguration.Attach(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Update Configuration: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Löschung einen Datensatz in der Tabelle Configuration
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

                AppConfigurationModel curEntry = await context.AppConfiguration.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        context.AppConfiguration.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Delete Configuration: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion
    }
}
