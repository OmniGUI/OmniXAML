namespace OmniXaml.Wpf
{
    // ReSharper disable once UnusedMember.Global
    public class DynamicResourceExtension : System.Windows.DynamicResourceExtension, IMarkupExtension
    {
        public DynamicResourceExtension()
        {            
        }

        public DynamicResourceExtension(object resourceKey)
        {
            ResourceKey = resourceKey;
        }


        public object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            var provideValue = base.ProvideValue(new ServiceLocator(markupExtensionContext));
            return provideValue;
        }
    }
}