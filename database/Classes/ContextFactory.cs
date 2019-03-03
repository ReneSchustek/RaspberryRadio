using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database.Classes
{
    /// <summary>
    /// Erzeug einen neuen DbContext
    /// </summary>
    public class ContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilider = new DbContextOptionsBuilder<DatabaseContext>();
            string connectionSting = CreateConnectionString.Create();
            optionsBuilider.UseSqlite(connectionSting);

            return new DatabaseContext(optionsBuilider.Options);
        }
    }
}
