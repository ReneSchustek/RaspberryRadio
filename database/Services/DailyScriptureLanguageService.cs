using Database.Classes;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Services
{
    public class DailyScriptureLanguageService
    {
        #region Models

        #endregion

        #region Constructor
        public DailyScriptureLanguageService() { }
        #endregion

        #region Create
        /// <summary>
        /// Speichern eines Datensatzes in der Tabelle DailyScriptureLanguage
        /// </summary>
        /// <param name="dailyScriptureLanguage">DailyScriptureLanguageModel</param>
        /// <returns>Id des gespeicherten Datensatzess</returns>
        public async Task<Int32> CreateAsync(DailyScriptureLanguageModel dailyScriptureLanguage)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (dailyScriptureLanguage == null) { return 0; }

            int result = 0;

            //Prüfen, ob Eintrag existiert
            DailyScriptureLanguageModel curEntry = await ReadByLanguageAsync(dailyScriptureLanguage.Language);
            if (curEntry != null) { return await UpdateAsync(dailyScriptureLanguage, curEntry.Id); }

            using (DatabaseContext context = db)
            {
                try
                {
                    dailyScriptureLanguage.CreatedDate = DateTime.Now;
                    dailyScriptureLanguage.UpdatedDate = DateTime.Now;
                    await context.DailyScriptureLanguage.AddAsync(dailyScriptureLanguage);
                    await context.SaveChangesAsync();

                    result = dailyScriptureLanguage.Id;
                }
                catch (Exception ex) { throw new Exception("Create DailyScriptureLanguage: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Read
        /// <summary>
        /// Lesen aller Datensätze der Tabelle DailyScriptureLanguage 
        /// </summary>
        /// <returns>IList aller Datensätze</returns>
        public async Task<IList<DailyScriptureLanguageModel>> ReadAllAsync()
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<DailyScriptureLanguageModel> result = null;
            using (DatabaseContext context = db)
            {
                try { result = await context.DailyScriptureLanguage.AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadAll DailyScriptureLanguage: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// Suchen eines Datensatzes nach Sprache in der Tabelle DailyScriptureLanguage
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<DailyScriptureLanguageModel> ReadByLanguageAsync(string language)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            //Keine Sprache angegeben           
            if ((language == null) || (language == String.Empty)) { throw new Exception("Es wurde keine Sprache angegeben, nach der gesucht werden soll."); }

            DailyScriptureLanguageModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.DailyScriptureLanguage.Where(x => x.Language == language).AsNoTracking().FirstOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadByLanguage DailyScriptureLanguage: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen eines Datensatzes nach Id in der Tabelle DailyScriptureLanguage
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<DailyScriptureLanguageModel> ReadByIdAsync(int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (id == 0) { throw new Exception("Es wurde eine ungültige Id angegeben."); }

            DailyScriptureLanguageModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.DailyScriptureLanguage.FindAsync(id); }
                catch (Exception ex) { throw new Exception("ReadById DailyScriptureLanguage: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update eines Datensatzes in der Tabelle DailyScriptureLanguage
        /// </summary>
        /// <param name="dailyScriptureLanguage">DailyScriptureLanguageModel</param>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Id des geänderten Datensatzes</returns>
        public async Task<Int32> UpdateAsync(DailyScriptureLanguageModel dailyScriptureLanguage, int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                DailyScriptureLanguageModel curEntry = await context.DailyScriptureLanguage.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        curEntry.Language = dailyScriptureLanguage.Language;
                        curEntry.UpdatedById = dailyScriptureLanguage.UpdatedById;
                        curEntry.UpdatedDate = DateTime.Now;
                        curEntry.Url = dailyScriptureLanguage.Url;

                        context.DailyScriptureLanguage.Attach(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Update DailyScriptureLanguage: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Löschung einen Datensatz in der Tabelle DailyScriptureLanguage und der Eintrag in der Tabelle DailyScripture
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
                DailyScriptureLanguageModel curEntry = await context.DailyScriptureLanguage.SingleOrDefaultAsync(x => x.Id == id);
                DailyScriptureModel dailyScriptureEntry = await context.DailyScripture.SingleOrDefaultAsync(x => x.Language == curEntry.Language);

                if (curEntry != null)
                {
                    try
                    {
                        //Tagestext auch löschen
                        context.DailyScripture.Remove(dailyScriptureEntry);
                        await context.SaveChangesAsync();

                        context.DailyScriptureLanguage.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Delete DailyScriptureLanguage: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion
    }
}
