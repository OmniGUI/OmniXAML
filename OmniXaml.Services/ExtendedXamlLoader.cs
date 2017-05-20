using System.Collections.Generic;
using System.Reflection;
using OmniXaml.Rework;

namespace OmniXaml.Services
{
    public class ExtendedXamlLoader : XamlLoader
    {
        public ExtendedXamlLoader(IList<Assembly> assemblies) : base(assemblies)
        {
        }

        protected override IValuePipeline GetValuePipeline(AttributeBasedMetadataProvider metadataProvider)
        {
            return new MarkupExtensionValuePipeline(new TemplatePipeline(new NoActionValuePipeline(), metadataProvider));
        }
    }
}