using System.Collections.Generic;
using System.Reflection;
using OmniXaml.Services;

namespace OmniXaml.Tests.Model
{
    using Metadata;

    public class ConstructionFragmentLoader : IConstructionFragmentLoader
    {
        public object Load(ConstructionNode node, IObjectBuilder builder, BuildContext buildContext)
        {
            return new TemplateContent(node, builder, buildContext);
        }

        public object Load(ConstructionNode node, IXamlLoader loader)
        {
            throw new System.NotImplementedException();
        }

        public object Load(ConstructionNode node)
        {
            return new TemplateContent(node, null, null);
        }
    }
}