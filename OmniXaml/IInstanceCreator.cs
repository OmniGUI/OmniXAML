namespace OmniXaml
{
    using System;

    public interface IInstanceCreator
    {
        CreationResult Create(Type type, CreationHints creationHints = null);
    }
}