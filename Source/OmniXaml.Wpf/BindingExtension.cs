namespace OmniXaml.Wpf
{
    using System.Windows;
    using System.Windows.Data;

    public class BindingExtension: Binding, IMarkupExtension
    {
        // ReSharper disable once UnusedMember.Global
        public BindingExtension()
        {            
        }

        // ReSharper disable once UnusedMember.Global
        public BindingExtension(string path)
        {
            Path = new PropertyPath(path);
        }     

        public object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            var provideValue = ProvideValue(new ServiceLocator(markupExtensionContext));           
            return provideValue;
        }
    }
}