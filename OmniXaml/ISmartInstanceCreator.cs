namespace OmniXaml
{
    using System;
    using System.Collections.Generic;

    public interface ISmartInstanceCreator
    {
        CreationResult Create(Type constructionNodeInstanceType, IEnumerable<InjectableMember> injectableMembers);
    }
}