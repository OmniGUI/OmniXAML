namespace OmniXaml
{
    using System;
    using Assembler;

    public interface IObjectAssembler
    {
        object Result { get; }
        EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        IWiringContext WiringContext { get; }

        void Process(XamlInstruction instruction);

        void OverrideInstance(object instance);
    }
}