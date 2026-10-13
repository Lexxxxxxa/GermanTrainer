using System.Windows;

namespace GermanTrainer
{
    public static class ThemeManager
    {
        private const string DarkThemeUri = "Themes/DarkTheme.xaml";
        private const string LightThemeUri = "Themes/LightTheme.xaml";

        private static bool _isDarkTheme = true;

        public static event Action? OnThemeChanged;

        public static bool IsDarkTheme => _isDarkTheme;

        public static void Initialize()
        {
            ApplyTheme();
        }

        public static void ToggleTheme()
        {
            _isDarkTheme = !_isDarkTheme;
            ApplyTheme();
            OnThemeChanged?.Invoke();
        }

        public static string GetThemeIcon() => _isDarkTheme ? "☀️" : "🌙";

        public static string GetThemeTooltip() =>
            _isDarkTheme ? "Переключить на светлую тему" : "Переключить на темную тему";

        private static void ApplyTheme()
        {
            var app = Application.Current;
            if (app == null) return;

            var dictionaries = app.Resources.MergedDictionaries;
            var newTheme = new ResourceDictionary
            {
                Source = new Uri(_isDarkTheme ? DarkThemeUri : LightThemeUri, UriKind.Relative)
            };

            var oldTheme = dictionaries.FirstOrDefault(d => d.Contains("IsThemeDictionary"));
            if (oldTheme != null)
                dictionaries.Remove(oldTheme);

            dictionaries.Insert(0, newTheme);
        }
    }
}
