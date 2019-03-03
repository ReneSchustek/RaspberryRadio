using Database.Classes;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Services
{
    /// <summary>
    /// Services für die Tabelle Token
    /// </summary>
    public class AppTokenService
    {
        #region Models

        #endregion

        #region Constructor
        public AppTokenService() { }
        #endregion

        #region Create
        /// <summary>
        /// Asynchrones Speichern eines Datensatzes in der Tabelle Token
        /// </summary>
        /// <param name="token">TokenModel</param>
        /// <returns>Id des gespeicherten Datensatzess</returns>
        public async Task<Int32> CreateAsync(AppTokenModel token)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (token == null) { return 0; }

            int result = 0;

            //Prüfen, ob Eintrag existiert
            IList<AppTokenModel> curEntries = await ReadAllAsync();
            if (curEntries.Count > 0) { return await UpdateAsync(token, 1); }

            using (DatabaseContext context = db)
            {
                try
                {
                    token.CreatedDate = DateTime.Now;
                    token.UpdatedDate = DateTime.Now;
                    await context.AppToken.AddAsync(token);
                    await context.SaveChangesAsync();

                    result = token.Id;
                }
                catch (Exception ex) { throw new Exception("Create Token: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Read
        /// <summary>
        /// Asynchrones Lesen aller Datensätze der Tabelle Token
        /// </summary>
        /// <returns>IList aller Datensätze</returns>
        public async Task<IList<AppTokenModel>> ReadAllAsync()
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<AppTokenModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.AppToken.AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadAll Token: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Asynchrones Suchen eines Datensatzes nach ID in der Tabelle Token
        /// </summary>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<AppTokenModel> ReadByIdAsync(int id)
        {
            if (id == 0) { throw new Exception("Es wurde eine fehlerhafte Id angegeben."); }

            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            AppTokenModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.AppToken.FindAsync(id); }
                catch (Exception ex) { throw new Exception("ReadById Token: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Asynchrones Update eines Datensatzes in der Tabelle Token
        /// </summary>
        /// <param name="token">TokenModel</param>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Id des geänderten Datensatzes</returns>
        public async Task<Int32> UpdateAsync(AppTokenModel token, int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                AppTokenModel curEntry = await context.AppToken.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        curEntry.DirbleToken = token.DirbleToken;
                        curEntry.OpenWeatherToken = token.OpenWeatherToken;
                        curEntry.UpdatedById = token.UpdatedById;
                        curEntry.UpdatedDate = DateTime.Now;

                        context.AppToken.Attach(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Update Token: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Asynchrone Löschung einen Datensatz in der Tabelle Token
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
                AppTokenModel curEntry = await context.AppToken.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        context.AppToken.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Delete Token: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion
    }
}
