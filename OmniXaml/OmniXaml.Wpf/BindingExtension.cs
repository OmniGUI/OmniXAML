namespace OmniXaml.Wpf
{
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

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
            var provideValue = ProvideValue(new ServiceLocator(markupExtensionContext));
            var be = provideValue as BindingExpression;
            if (be != null)
            {
                var targetObject = be.Target;
                var property = be.TargetProperty;

            }
            return provideValue;
        }
    }
}