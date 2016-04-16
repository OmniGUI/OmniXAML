namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using Glass.Core;

    public interface IContentPropertyProvider : IAdd<ContentPropertyDefinition>, IEnumerable<ContentPropertyDefinition>
    {
        string GetContentPropertyName(Type type);
    }
}