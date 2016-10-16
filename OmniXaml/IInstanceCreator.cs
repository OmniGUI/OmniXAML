namespace OmniXaml
{
    using System;
    using System.Collections.Generic;

    public interface IInstanceCreator
    {
        object Create(Type type, IEnumerable<InjectableMember> injectableMembers = null);
    }
}