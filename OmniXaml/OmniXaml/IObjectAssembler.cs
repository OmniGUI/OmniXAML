namespace OmniXaml
{
    using System;
    using Assembler;

    public interface IObjectAssembler
    {
        object Result { get; }
        EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        WiringContext WiringContext { get; }

        void WriteNode(XamlNode node);

        void OverrideInstance(object instance);
    }
}