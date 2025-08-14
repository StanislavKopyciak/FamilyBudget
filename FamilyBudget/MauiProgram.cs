using FamilyBudget.Services;
using Microsoft.Extensions.Logging;
using FamilyBudget.Data;
using Microcharts.Maui;

namespace FamilyBudget
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMicrocharts();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // SQLite DB (локальна база даних)
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "familybudget.db3");
            builder.Services.AddSingleton<Database>(s => new Database(dbPath));

            return builder.Build();
        }
    }
}
