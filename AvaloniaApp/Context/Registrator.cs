namespace AvaloniaApp.Context
{
    using System.Collections.Generic;
    using System.Globalization;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml.Converters;
    using Avalonia.Media.Imaging;
    using OmniXaml;
    using OmniXaml.ObjectAssembler.Commands;
    using OmniXaml.TypeConversion;
    using OmniXaml.Typing;
    using SharpDX.Direct2D1;

    public static class Registrator
    {
        public static ISourceValueConverter GetSourceValueConverter()
        {
            var context = new MyHackedContext();
            

            var sourceValueConverter = new SourceValueConverter();
            sourceValueConverter.Add(typeof(Thickness), value => new ThicknessTypeConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(Brush), value => new BrushTypeConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(GridLength), value => new GridLengthTypeConverter().ConvertFrom(null, CultureInfo.CurrentCulture, value));
            sourceValueConverter.Add(typeof(IBitmap), value => new BitmapTypeConverter().ConvertFrom(context, CultureInfo.CurrentCulture, value));
            return sourceValueConverter;
        }
    }

    public class MyHackedContext : IValueContext
    {
        public MyHackedContext()
        {
            ParsingDictionary = new Dictionary<string, object>() {{"Uri", "\\"}};
        }

        public ITypeRepository TypeRepository { get; }
        public ITopDownValueContext TopDownValueContext { get; }
        public IReadOnlyDictionary<string, object> ParsingDictionary { get; } 
    }
}