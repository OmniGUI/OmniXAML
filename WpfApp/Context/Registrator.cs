namespace WpfApplication1.Context
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using OmniXaml;

    public static class Registrator
    {
        public static ISourceValueConverter GetSourceValueConverter()
        {
            var sourceValueConverter = new SourceValueConverter();
            sourceValueConverter.Add(typeof(Thickness), (value, b) => new ThicknessConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(Brush), (value, b) => new BrushConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(GridLength), (value, b) => new GridLengthConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(ImageSource), (value, b) => new ImageSourceConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            return sourceValueConverter;
        }
    }
}