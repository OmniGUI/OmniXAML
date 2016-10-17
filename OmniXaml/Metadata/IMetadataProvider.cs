namespace OmniXaml.Metadata
{
    using System;

    public interface IMetadataProvider
    {
        Metadata Get(Type type);
    }
}