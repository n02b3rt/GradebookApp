using System.Windows;
using GradebookApp.Services;
using GradebookApp.Views;
using GradebookApp.Data;
using Microsoft.EntityFrameworkCore;

namespace GradebookApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var config = AppSettingsService.LoadDatabaseConfig();

            if (config != null)
            {
                var connectionString = $"Host={config.Host};Port={config.Port};Database={config.DatabaseName};Username={config.Username};Password={config.Password}";
                var options = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options;

                using var db = new AppDbContext(options);
                if (db.Database.CanConnect())
                {
                    new MainWindow().Show();
                    return;
                }
            }

            // Jeśli nie ma configu albo nie działa połączenie – pokaż okno logowania
            new ConnectionWindow().Show();
        }
    }
}
