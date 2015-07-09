namespace OmniXaml
{
    using System;
    using Assembler;
    using NewAssembler;

    public interface IObjectAssembler
    {
        object Result { get; }
        EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        WiringContext WiringContext { get; }

        void Process(XamlNode node);

        void OverrideInstance(object instance);
    }
}