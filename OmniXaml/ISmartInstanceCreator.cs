namespace OmniXaml
{
    using System;
    using Rework;

    public interface ISmartInstanceCreator
    {
        CreationResult Create(Type type, CreationHints creationHints = null);
    }
}