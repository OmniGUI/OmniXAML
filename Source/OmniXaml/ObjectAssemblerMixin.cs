namespace OmniXaml
{
    using System.Collections.Generic;

    public static class ObjectAssemblerMixin
    {
        public static void Process(this IObjectAssembler assembler, IEnumerable<XamlInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                assembler.Process(instruction);
            }
        }
    }
}