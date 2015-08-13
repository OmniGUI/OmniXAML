namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using Builder;

    public interface IContentPropertyProvider : IAdd<ContentPropertyDefinition>, IEnumerable<ContentPropertyDefinition>
    {
        string GetContentPropertyName(Type type);
    }
}