namespace AvaloniaApp.Context
{
    using System.Globalization;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml.Converters;
    using Avalonia.Media.Imaging;
    using OmniXaml;
    using SharpDX.Direct2D1;

    public static class Registrator
    {
        public static ISourceValueConverter GetSourceValueConverter()
        {
            var context = new HackedValueContext();
            
            var sourceValueConverter = new SourceValueConverter();
            sourceValueConverter.Add(typeof(Thickness), value => new ThicknessTypeConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(Brush), value => new BrushTypeConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(GridLength), value => new GridLengthTypeConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(IBitmap), value => new BitmapTypeConverter().ConvertFrom(context, CultureInfo.CurrentCulture, value));
            return sourceValueConverter;
        }
    }
}