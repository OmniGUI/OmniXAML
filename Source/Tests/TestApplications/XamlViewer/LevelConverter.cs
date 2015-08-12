namespace XamlViewer
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class LevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var level = (int)value;
            return new Thickness(level * 25, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}