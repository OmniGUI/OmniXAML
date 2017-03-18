namespace OmniXaml
{
    using System;

    public interface ISmartInstanceCreator
    {
        CreationResult Create(Type type, CreationHints creationHints);
    }
}