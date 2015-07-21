namespace OmniXaml.Wpf
{
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    public class BindingExtension: Binding, IMarkupExtension, IProvideValueTarget
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

        public RelativeSource RelativeSource { get; set; }

        public object TargetObject { get; }
        public object TargetProperty { get; }
    }
}