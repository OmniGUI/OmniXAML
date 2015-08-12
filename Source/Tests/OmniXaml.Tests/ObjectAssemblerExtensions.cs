namespace OmniXaml.Tests
{
    using System.Collections.Generic;

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