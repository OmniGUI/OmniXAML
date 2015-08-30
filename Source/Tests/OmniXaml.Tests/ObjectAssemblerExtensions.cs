namespace OmniXaml.Tests
{
    using System.Collections.Generic;

    static internal class ObjectAssemblerExtensions
    {
        public static void PumpNodes(this IObjectAssembler assembler, IEnumerable<XamlInstruction> instructions)
        {
            foreach (var instruction in instructions) {
                assembler.Process(instruction);
            }
        }
    }
}