using Database.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Database.Model
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (DatabaseContext context = new DatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                if (context.AppInfo.Any() && context.DailyScriptureLanguage.Any()) { return; }

                if (!context.AppInfo.Any())
                {
                    context.AppInfo.AddRange(
                        new AppInfoModel
                        {
                            Info = "RadioStation by René Schustek - (c) 2019",
                            Software = "Datenbankengine: sqlite",
                            CreatedById = 0,
                            CreatedDate = DateTime.Now,
                            UpdatedById = null,
                            UpdatedDate = null
                        });

                    context.SaveChanges();
                }

                if (!context.DailyScriptureLanguage.Any())
                {
                    context.DailyScriptureLanguage.AddRange(
                        new DailyScriptureLanguageModel
                        {
                            Language = "Deutsch",
                            Url = "https://wol.jw.org/de/wol/dt/r10/lp-x",
                            CreatedById = 0,
                            CreatedDate = DateTime.Now,
                            UpdatedById = null,
                            UpdatedDate = null
                        });

                    context.SaveChanges();
                }
            }
        }
    }
}
