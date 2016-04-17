namespace OmniXaml.Wpf
{
    using System.Windows;

    // ReSharper disable once UnusedMember.Global
    public class StaticResourceExtension : IMarkupExtension
    {
        public StaticResourceExtension()
        {            
        }

        public StaticResourceExtension(object resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public object ResourceKey { get; set; }

        public object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            var type = markupExtensionContext.ValueContext.TypeRepository.GetByType(typeof (ResourceDictionary));
            var resourceDictionary = (ResourceDictionary) markupExtensionContext.ValueContext.TopDownValueContext.GetLastInstance(type);
            return resourceDictionary[ResourceKey];
        }
    }
}