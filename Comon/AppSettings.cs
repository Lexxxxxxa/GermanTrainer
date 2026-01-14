using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GermanTrainer
{
    public static class AppSettings
    {
        public static List<int> EnabledLessons { get; set; } = new();
        public static List<string> EnabledWordTypes { get; set; } = new();

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GermanArticlesTrainer",
            "settings.json"
        );

        static AppSettings()
        {
            LoadSettings();
        }

        public static void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    EnabledLessons.Clear();
                    if (root.TryGetProperty("EnabledLessons", out var lessonsArray))
                    {
                        foreach (var lesson in lessonsArray.EnumerateArray())
                        {
                            if (lesson.TryGetInt32(out int lessonNum))
                            {
                                EnabledLessons.Add(lessonNum);
                            }
                        }
                    }

                    EnabledWordTypes.Clear();
                    if (root.TryGetProperty("EnabledWordTypes", out var typesArray))
                    {
                        foreach (var type in typesArray.EnumerateArray())
                        {
                            if (type.ValueKind == JsonValueKind.String)
                            {
                                EnabledWordTypes.Add(type.GetString() ?? "");
                            }
                        }
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
            try
            {
                var directory = Path.GetDirectoryName(SettingsPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory ?? "");
                }

                var settings = new
                {
                    EnabledLessons = EnabledLessons,
                    EnabledWordTypes = EnabledWordTypes
                };

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}