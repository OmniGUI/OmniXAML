namespace OmniXaml.Wpf
{
    using System.Windows;
    using System.Windows.Data;

    public class BindingExtension: Binding, IMarkupExtension
    {
        public BindingExtension()
        {            
        }

        public BindingExtension(string path)
        {
            Path = new PropertyPath(path);
        }     

        public object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return ProvideValue(new ServiceLocator(markupExtensionContext));
        }
    }
}