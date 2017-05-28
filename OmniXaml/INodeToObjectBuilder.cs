using System.Collections.Generic;

namespace OmniXaml
{
    public interface INodeToObjectBuilder
    {
        object Build(ConstructionNode node, BuilderContext context = null);
    }

    public class BuilderContext
    {
        public IDictionary<string, object> Store { get; set; } = new Dictionary<string, object>();
    }
}