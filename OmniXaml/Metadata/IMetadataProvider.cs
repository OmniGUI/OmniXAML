namespace OmniXaml.Metadata
{
    using System;
    using System.Collections.Generic;

    public interface IMetadataProvider
    {
        Metadata Get(Type type);
    }
}