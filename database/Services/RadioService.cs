using Database.Classes;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Database.Services
{
    public class RadioService
    {
        #region Models
        #endregion

        #region Constructor
        public RadioService() { }
        #endregion

        #region Create
        /// <summary>
        /// Speichern eines Datensatzes in der Tabelle RadioFavorites
        /// </summary>
        /// <param name="radioFav">RadioFavModel</param>
        /// <returns>Id des gespeicherten Datensatzes</returns>
        public async Task<Int32> CreateAsync(RadioFavModel radioFav)
        {
            ContextFactory contextFactory = new ContextFactory();

            DatabaseContext db = contextFactory.CreateDbContext(null);

            if (radioFav == null) { return 0; }

            int result = 0;

            //Prüfen, ob Eintrag existiert
            using (var context = db)
            {
                try
                {
                    RadioFavModel curEntries = await context.RadioFavorites.Where(x => x.Url == radioFav.Url).AsNoTracking().FirstOrDefaultAsync();
                    if (curEntries != null) { return await UpdateAsync(radioFav, curEntries.Id); }

                    curEntries = null;
                    curEntries = await context.RadioFavorites.Where(x => x.Pos == radioFav.Pos).AsNoTracking().FirstOrDefaultAsync();
                    if(curEntries != null) { return await UpdateAsync(radioFav, curEntries.Id); }

                    radioFav.CreatedDate = DateTime.Now;
                    radioFav.UpdatedDate = DateTime.Now;
                    await context.RadioFavorites.AddAsync(radioFav);
                    await context.SaveChangesAsync();

                    result = radioFav.Id;
                }
                catch (Exception ex) { throw new Exception("Create RadioFavorites: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Read
        /// <summary>
        /// Lesen aller Datensätze der Tabelle RadioFavorites 
        /// </summary>
        /// <returns>IList aller Datensätze</returns>
        public async Task<IList<RadioFavModel>> ReadAllAsync()
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<RadioFavModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.RadioFavorites.OrderBy(x => x.Pos).AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadAll RadioFavorites: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Liest die ersten x Positionen aus der Tabelle RadioFavorites
        /// </summary>
        /// <param name="number">Anzahl der Datensätze</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<IList<RadioFavModel>> ReadFirst(int number)
        {
            if (number == 0) { return new List<RadioFavModel>(); }

            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            IList<RadioFavModel> result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.RadioFavorites.OrderBy(x => x.Pos).Take(number).AsNoTracking().ToListAsync(); }
                catch (Exception ex) { throw new Exception("ReadFirst RadioFavorites: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }


        /// <summary>
        /// Liest einen Datensatz nach der Id in der Tabelle RadioFavorites aus
        /// </summary>
        /// <param name="id">Id des eintrags</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<RadioFavModel> ReadByIdAsync(int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            RadioFavModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.RadioFavorites.Where(x => x.Id == id).AsNoTracking().SingleOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadById RadioFavorites: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Liest einen Datensatz nach dem Namen in der Tabelle RadioFavorites aus
        /// </summary>
        /// <param name="name">Suchstring des Eintrags</param>
        /// <returns>Gefundener Datensatz</returns>
        public async Task<RadioFavModel> ReadByNameAsync(string name)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            RadioFavModel result = null;

            using (DatabaseContext context = db)
            {
                try { result = await context.RadioFavorites.Where(x => x.Name.Contains(name)).AsNoTracking().SingleOrDefaultAsync(); }
                catch (Exception ex) { throw new Exception("ReadById RadioFavorites: " + ex.ToString()); }
            }

            db.Dispose();

            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update eines Datensatzes in der Tabelle RadioFavorites
        /// </summary>
        /// <param name="radioFav">RadioFavModel</param>
        /// <param name="id">Id des Eintrags</param>
        /// <returns>Id des geänderten Datensatzes</returns>
        public async Task<Int32> UpdateAsync(RadioFavModel radioFav, int id)
        {
            ContextFactory contextFactory = new ContextFactory();
            DatabaseContext db = contextFactory.CreateDbContext(null);

            int result = 0;

            using (DatabaseContext context = db)
            {
                RadioFavModel curEntry = await context.RadioFavorites.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        curEntry.Name = radioFav.Name;
                        curEntry.Pos = radioFav.Pos;
                        curEntry.Url = radioFav.Url;
                        curEntry.FavoriteName = radioFav.FavoriteName;
                        curEntry.Name = radioFav.Name;
                        curEntry.ImageUrl = radioFav.ImageUrl;
                        curEntry.UpdatedById = radioFav.UpdatedById;
                        curEntry.UpdatedDate = DateTime.Now;

                        context.RadioFavorites.Attach(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Update RadioFavorites: " + ex.ToString()); }
                }
            }

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Löschung einen Datensatz in der Tabelle RadioFavorites
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
                RadioFavModel curEntry = await context.RadioFavorites.SingleOrDefaultAsync(x => x.Id == id);

                if (curEntry != null)
                {
                    try
                    {
                        context.RadioFavorites.Remove(curEntry);
                        await context.SaveChangesAsync();

                        result = curEntry.Id;
                    }
                    catch (Exception ex) { throw new Exception("Delete RadioFavorites: " + ex.ToString()); }
                }
            }

            db.Dispose();

            return result;
        }
        #endregion
    }
}
