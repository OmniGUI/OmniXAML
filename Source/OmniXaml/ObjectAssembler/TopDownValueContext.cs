namespace OmniXaml.ObjectAssembler
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Commands;
    using Typing;

    public class TopDownValueContext : ITopDownValueContext
    {
        private IList<StoredInstance> Store { get; } = new List<StoredInstance>();
        public void Add(object instance, XamlType xamlType)
        {
            Store.Add(new StoredInstance(instance, xamlType));
        }

        public object GetLastInstance(XamlType xamlType)
        {
            var lastStored = Store.Last(stored => Equals(stored.XamlType, xamlType));

            return lastStored.Instance;
        }

        public IReadOnlyCollection<StoredInstance> StoredInstances => new ReadOnlyCollection<StoredInstance>(Store);
    }
}