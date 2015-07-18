namespace OmniXaml.Wpf
{
    using System.Windows.Data;

    public class BindingExtension: Binding, IMarkupExtension
    {        
        public object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return ProvideValue(new ServiceLocator(markupExtensionContext));
        }
    }
}