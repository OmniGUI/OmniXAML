namespace OmniXaml.Tests.Rework
{
    using System;
    using OmniXaml.Rework;

    internal class SimpleInstanceCreator : ISmartInstanceCreator
    {
        public CreationResult Create(Type type, CreationHints creationHints)
        {
            var instance = Activator.CreateInstance(type);
            return new CreationResult(instance, new CreationHints());
        }
    }
}