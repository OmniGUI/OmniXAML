using System;

namespace OmniXaml.Services
{
    public class SimpleInstanceCreator : IInstanceCreator
    {
        public CreationResult Create(Type type, CreationHints creationHints)
        {
            var instance = Activator.CreateInstance(type);
            return new CreationResult(instance, new CreationHints());
        }
    }
}