using System.Collections.Generic;

namespace OmniXaml.Tests
{
    static internal class ObjectAssemblerExtensions
    {
        public static void PumpNodes(this IObjectAssembler assembler, IEnumerable<XamlNode> nodes)
        {
            foreach (var xamlNode in nodes)
            {
                assembler.WriteNode(xamlNode);
            }
        }
    }
}