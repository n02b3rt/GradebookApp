using System.IO;
using System.Text.Json;
using GradebookApp.Models;

namespace GradebookApp.Services
{
    public static class AppSettingsService
    {
        private static readonly string FilePath = "appsettings.json";

        public static void SaveDatabaseConfig(DatabaseConfig config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static DatabaseConfig? LoadDatabaseConfig()
        {
            if (!File.Exists(FilePath)) return null;

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<DatabaseConfig>(json);
        }
    }
}
