namespace OmniXaml
{
    using System;
    using ObjectAssembler;

    public interface IObjectAssembler
    {
        object Result { get; }
        EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        IWiringContext WiringContext { get; }
        InstanceLifeCycleHandler InstanceLifeCycleHandler { get; set; }

        void Process(XamlInstruction instruction);

        void OverrideInstance(object instance);      
    }
}