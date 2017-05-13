using System.Collections.Generic;
using System.Reflection;
using OmniXaml.Rework;
using OmniXaml.Services;

namespace XamlLoadTest
{
    internal class ExtendedXamlLoader : XamlLoader
    {
        public ExtendedXamlLoader(IList<Assembly> assemblies) : base(assemblies)
        {
        }

        protected override IValuePipeline GetValuePipeline()
        {
            return new MarkupExtensionValuePipeline(new NoActionValuePipeline());
        }
    }
}