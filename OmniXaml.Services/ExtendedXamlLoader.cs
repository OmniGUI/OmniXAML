using System.Collections.Generic;
using System.Reflection;

namespace OmniXaml.Services
{
    public class ExtendedXamlLoader : BasicXamlLoader
    {
        public ExtendedXamlLoader(IList<Assembly> assemblies) : base(assemblies)
        {            
        }

        protected override IValuePipeline ValuePipeline => new MarkupExtensionValuePipeline(new TemplatePipeline(new NoActionValuePipeline(), MetadataProvider));
    }
}