namespace OmniXaml
{
    using System.Collections.Generic;

    public class CreationResult
    {
        public CreationResult(object instance, IEnumerable<InjectableMember> injectedMembers)
        {
            Instance = instance;
            InjectedMembers = injectedMembers;
        }

        public object Instance { get; }
        public IEnumerable<InjectableMember> InjectedMembers { get; }
    }
}