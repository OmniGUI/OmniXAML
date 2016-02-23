namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections.Generic;
    using Typing;

    public interface ITopDownValueContext
    {
        void Add(object instance, XamlType xamlType);
        object GetLastInstance(XamlType xamlType);
        IReadOnlyCollection<StoredInstance> StoredInstances { get; }
    }
}