using System.Collections.Generic;
using System.Reflection;
using OmniXaml.Rework;

namespace OmniXaml.Services
{
    public class XamlLoaderExtended : XamlLoaderSimple
    {
        public XamlLoaderExtended(List<Assembly> assemblies) : base((IList<Assembly>) assemblies)
        {            
        }

        protected override IValuePipeline ValuePipeline => new MarkupExtensionValuePipeline(new TemplatePipeline(new NoActionValuePipeline(), MetadataProvider));
    }
}