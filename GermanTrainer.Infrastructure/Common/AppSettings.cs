using System.IO;
using System.Text.Json;
using GermanTrainer.Models;

namespace GermanTrainer
{
    public static class AppSettings
    {
        public static List<int> EnabledLessons { get; set; } = new();
        public static List<string> EnabledWordTypes { get; set; } = new();

        private static string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GermanArticlesTrainer",
            "settings.json"
        );

        static AppSettings()
        {
            LoadSettings();
        }

        public static bool IsLessonEnabled(int lesson) =>
            EnabledLessons.Count == 0 || EnabledLessons.Contains(lesson);

        public static bool IsWordTypeEnabled(WordType type) =>
            EnabledWordTypes.Count == 0 || EnabledWordTypes.Contains(type.ToSettingsKey());

        public static void LoadSettings()
        {
            try
            {
                if (!File.Exists(SettingsPath))
                    return;

                var json = File.ReadAllText(SettingsPath);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                EnabledLessons.Clear();
                if (root.TryGetProperty(nameof(EnabledLessons), out var lessonsArray) &&
                    lessonsArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var lesson in lessonsArray.EnumerateArray())
                    {
                        if (lesson.TryGetInt32(out int lessonNum))
                            EnabledLessons.Add(lessonNum);
                    }
                }

                EnabledWordTypes.Clear();
                if (root.TryGetProperty(nameof(EnabledWordTypes), out var typesArray) &&
                    typesArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var type in typesArray.EnumerateArray())
                    {
                        if (type.ValueKind == JsonValueKind.String)
                            EnabledWordTypes.Add(type.GetString() ?? string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }
        }

        public static void SaveSettings()
        {
                var directory = Path.GetDirectoryName(SettingsPath);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                var settings = new
                {
                    EnabledLessons,
                    EnabledWordTypes
                };

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                var temp = SettingsPath + ".tmp";
                try
                {
                    File.WriteAllText(temp, json);
                    File.Move(temp, SettingsPath, overwrite: true);
                }
                finally { if (File.Exists(temp)) File.Delete(temp); }
                // Let the UI report failed writes; never announce a silent false success.
        }
    }
}
