namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using Glass;

    public interface IContentPropertyProvider : IAdd<ContentPropertyDefinition>, IEnumerable<ContentPropertyDefinition>
    {
        string GetContentPropertyName(Type type);
    }
}