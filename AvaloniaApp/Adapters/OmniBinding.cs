namespace AvaloniaApp.Adapters
{
    extern alias OmniXamlV2;
    using Avalonia.Markup.Xaml.Data;
    using Avalonia.Markup.Xaml.MarkupExtensions;
    using IMarkupExtension = OmniXamlV2.OmniXaml.IMarkupExtension;

    public class OmniBinding : IMarkupExtension
    {
        public object GetValue()
        {
            var bindingExtension = new BindingExtension
            {
                Path = Path
            };

            var provideValue = bindingExtension.ProvideValue(null);

            return provideValue;
        }

        public string Path { get; set; }
    }
}