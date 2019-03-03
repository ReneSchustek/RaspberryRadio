using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Classes
{
    /// <summary>
    /// Database Context für die Localdb
    /// </summary>
    public class DatabaseContext : DbContext
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Options</param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        #endregion

        #region Models
        public DbSet<AppConfigurationModel> AppConfiguration { get; set; }
        public DbSet<AppInfoModel> AppInfo { get; set; }
        public DbSet<AppTokenModel> AppToken { get; set; }

        public DbSet<DailyScriptureModel> DailyScripture { get; set; }
        public DbSet<DailyScriptureLanguageModel> DailyScriptureLanguage { get; set; }        

        public DbSet<OpenWeatherCityModel> OpenWeatherCity { get; set; }
        public DbSet<OpenWeatherSavedCitiesModel> OpenWeatherSavedCities { get; set; }

        public DbSet<CalendarModel> Calendars { get; set; }
        #endregion

        #region Builder
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        #endregion
    }
}
