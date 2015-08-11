using System.Collections.Generic;

namespace OmniXaml.Tests
{
    static internal class ObjectAssemblerExtensions
    {
        public static void PumpNodes(this IObjectAssembler assembler, IEnumerable<XamlInstruction> nodes)
        {
            foreach (var xamlNode in nodes)
            {
                assembler.Process(xamlNode);
            }
        }
    }
}