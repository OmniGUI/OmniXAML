namespace OmniXaml
{
    using System;
    using ObjectAssembler;
    using ObjectAssembler.Commands;

    public interface IObjectAssembler
    {
        object Result { get; }
        EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        InstanceLifeCycleHandler InstanceLifeCycleHandler { get; set; }
        IRuntimeTypeSource TypeSource { get; }
        ITopDownValueContext TopDownValueContext { get; }

        void Process(Instruction instruction);

        void OverrideInstance(object instance);      
    }
}