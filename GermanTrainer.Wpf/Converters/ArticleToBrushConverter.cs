using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GermanTrainer.Converters
{
    public class ArticleToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = (value as string)?.ToLowerInvariant() switch
            {
                "der" => "DerBrush",
                "die" => "DieBrush",
                "das" => "DasBrush",
                _ => "NeutralArticleBrush"
            };

            return Application.Current?.TryFindResource(key) as Brush ?? Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
