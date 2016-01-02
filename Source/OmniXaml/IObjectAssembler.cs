namespace OmniXaml
{
    using System;
    using ObjectAssembler;

    public interface IObjectAssembler
    {
        object Result { get; }
        EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        InstanceLifeCycleHandler InstanceLifeCycleHandler { get; set; }
        ITypeContext TypeContext { get; }

        void Process(XamlInstruction instruction);

        void OverrideInstance(object instance);      
    }
}