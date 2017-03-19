namespace OmniXaml.Tests.Rework
{
    using System;
    using OmniXaml.Rework;

    internal class SmartInstanceCreatorMock : ISmartInstanceCreator
    {
        private Func<Type, CreationHints, CreationResult> createFunc = (type, hints) => new CreationResult(Activator.CreateInstance(type), new CreationHints()); 

        public void SetObjectFactory(Func<Type, CreationHints, CreationResult> factory)
        {
            this.createFunc = factory;
        }

        public CreationResult Create(Type type, CreationHints creationHints)
        {
            return createFunc(type, creationHints);
        }
    }
}