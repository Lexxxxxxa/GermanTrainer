using System;
using System.Windows.Media;

namespace GermanTrainer
{
    public static class ThemeManager
    {
        // ============ ТЕМА ПО УМОЛЧАНИЮ - ТЕМНАЯ ============
        private static bool _isDarkTheme = true;  // ✅ Темная тема по умолчанию

        public static event Action? OnThemeChanged;

        public static void ToggleTheme()
        {
            _isDarkTheme = !_isDarkTheme;
            OnThemeChanged?.Invoke();
        }

        public static bool IsDarkTheme => _isDarkTheme;

        // ============ ЦВЕТА ФОНА ============

        public static Brush GetBackground()
        {
            return _isDarkTheme
                ? new SolidColorBrush(Color.FromRgb(31, 33, 33))      // Темный фон
                : new SolidColorBrush(Color.FromRgb(252, 252, 249));  // Светлый фон
        }

        public static Brush GetSurface()
        {
            return _isDarkTheme
                ? new SolidColorBrush(Color.FromRgb(38, 40, 40))      // Темная поверхность
                : new SolidColorBrush(Color.FromRgb(255, 255, 253));  // Светлая поверхность
        }

        // ============ ЦВЕТА ТЕКСТА ============

        public static Brush GetText()
        {
            return _isDarkTheme
                ? new SolidColorBrush(Color.FromRgb(245, 245, 245))   // ✅ Светлый текст на темном фоне
                : new SolidColorBrush(Color.FromRgb(19, 52, 59));     // Темный текст на светлом фоне
        }

        public static Brush GetTextSecondary()
        {
            return _isDarkTheme
                ? new SolidColorBrush(Color.FromArgb(179, 167, 169, 169))  // ✅ Серый текст (70% opacity)
                : new SolidColorBrush(Color.FromRgb(98, 108, 113));         // Темно-серый текст
        }

        // ============ ЦВЕТА ГРАНИЦ ============

        public static Brush GetBorder()
        {
            return _isDarkTheme
                ? new SolidColorBrush(Color.FromArgb(76, 119, 124, 124))   // Серая граница (30% opacity)
                : new SolidColorBrush(Color.FromArgb(51, 94, 82, 64));     // Коричневая граница (20% opacity)
        }

        // ============ ЦВЕТА СТАТИСТИКИ ============

        public static Brush GetCorrectBg()
        {
            return new SolidColorBrush(Color.FromRgb(76, 175, 80));  // Зеленый
        }

        public static Brush GetWrongBg()
        {
            return new SolidColorBrush(Color.FromRgb(244, 67, 54));  // Красный
        }

        public static Brush GetPercentBg()
        {
            return new SolidColorBrush(Color.FromRgb(33, 150, 243));  // Синий
        }

        // ============ ИКОНКА КНОПКИ ТЕМЫ ============

        public static string GetThemeIcon()
        {
            return _isDarkTheme ? "☀️" : "🌙";  // Солнце для темной темы, луна для светлой
        }

        public static string GetThemeTooltip()
        {
            return _isDarkTheme ? "Переключить на светлую тему" : "Переключить на темную тему";
        }
    }
}