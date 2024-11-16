using System.Globalization;
using System.Windows.Data;

namespace WallpaperTimeSheet.Classes
{
    public class TimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("HH:mm"); // Formato 24 ore
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string input && DateTime.TryParseExact(input, "HH:mm", culture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            return System.Windows.Data.Binding.DoNothing; // Non esegue l'update se il formato è errato
        }
    }

}
