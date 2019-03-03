using Database.Classes;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Services
{
    public class DailyScriptureService
    {
        #region Models

        #endregion

        #region Constructor
        public DailyScriptureService() { }
        #endregion

        #region Create
        /// <summary>
        /// Speichern eines Datensatzes in der Tabelle DailyScripture
        /// </summary>
        /// <param name="dailyScripture">DailyScriptureModel</param>
        /// <returns>Id des gespeicherten Datensatzess</returns>
        public async Task<Int32> CreateAsync(DailyScriptureModel dailyScripture)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (dailyScripture == null) { return 0; }

            int result = 0;

            //Prüfen, ob Eintrag existiert
            DailyScriptureModel curEntry = await ReadByLanguageAsync(dailyScripture.Language);
            if (curEntry != null) { return await UpdateAsync(dailyScripture, curEntry.Id); }

            using (DatabaseContext context = db)
            {
                try
                {
                    dailyScripture.CreatedDate = DateTime.Now;
                    dailyScripture.UpdatedDate = DateTime.Now;
                    await context.DailyScripture.AddAsync(dailyScripture);
                    await context.SaveChangesAsync();

                    result = dailyScripture.Id;
                }
                catch (Exception ex) { throw new Exception("Create DailyScripture: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Read
        /// <summary>
        /// Lesen aller Datensätze der Tabelle DailyScripture 
        /// </summary>
        /// <returns>IList aller Datensätze</returns>
        public async Task<IList<DailyScriptureModel>> ReadAllAsync()
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<DailyScriptureModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.DailyScripture.AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadAll DailyScripture: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen eines Datensatzes nach Titel und Sprache in der Tabelle DailyScripture
        /// </summary>
        /// <param name="title">Titel</param>
        /// <param name="language">Language</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<DailyScriptureModel> ReadByTitleLanguageAsync(string title, string language)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            //Kein Titel oder Sprache angegeben
            if ((title == null) || (title == String.Empty)) { throw new Exception("Es wurde kein Titel angegeben, nach dem gesucht werden soll."); }
            if ((language == null) || (language == String.Empty)) { throw new Exception("Es wurde keine Sprache angegeben, nach der gesucht werden soll."); }

            DailyScriptureModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.DailyScripture.Where(x => x.Title == title && x.Language == language).AsNoTracking().FirstOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadByTitleLanguage DailyScripture: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Suchen eines Datensatzes nach Sprache in der Tabelle DailyScripture
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<DailyScriptureModel> ReadByLanguageAsync(string language)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            //Keine Sprache angegeben
            if ((language == null) || (language == String.Empty)) { throw new Exception("Es wurde keine Sprache angegeben, nach der gesucht werden soll."); }

            DailyScriptureModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.DailyScripture.Where(x => x.Language == language).AsNoTracking().FirstOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadByLanguage DailyScripture: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update eines Datensatzes in der Tabelle DailyScripture
        /// </summary>
        /// <param name="dailyScripture">DailyScriptureModel</param>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Id des geänderten Datensatzes</returns>
        public async Task<Int32> UpdateAsync(DailyScriptureModel dailyScripture, int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                DailyScriptureModel curEntry = await context.DailyScripture.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        curEntry.Comment = dailyScripture.Comment;
                        curEntry.Language = dailyScripture.Language;
                        curEntry.Publication = dailyScripture.Publication;
                        curEntry.Text = dailyScripture.Text;
                        curEntry.Title = dailyScripture.Title;
                        curEntry.UpdatedById = dailyScripture.UpdatedById;
                        curEntry.UpdatedDate = DateTime.Now;

                        context.DailyScripture.Update(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Update DailyScripture: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Löschung einen Datensatz in der Tabelle DailyScripture
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
                DailyScriptureModel curEntry = await context.DailyScripture.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        context.DailyScripture.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Delete DailyScripture: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Löschung der Datensätze in der Tabelle DailyScripture nach Sprache
        /// </summary>
        /// <param name="Language">Die Sprache</param>
        /// <returns>ID des gelöschten Datensatzes</returns>
        public async Task<Int32> DeleteByLanguageAsync(string language)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;
            DailyScriptureModel curEntry = await db.DailyScripture.SingleOrDefaultAsync(x => x.Language == language);

            if (curEntry != null)
            {
                using (DatabaseContext context = db)
                {
                    try
                    {
                        context.DailyScripture.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("DeleteByLanguage DailyScripture: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion
    }
}
