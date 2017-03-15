namespace OmniXaml.Tests.Rework
{
    using System;
    using System.Collections.Generic;

    internal class SmartInstanceCreatorMock : ISmartInstanceCreator
    {
        private Func<Type, IEnumerable<InjectableMember>, CreationResult> createFunc = (type, members) => new CreationResult(Activator.CreateInstance(type), new InjectableMember[0]);

        public CreationResult Create(Type type, IEnumerable<InjectableMember> injectableMembers = null)
        {
            return createFunc(type, injectableMembers);
        }

        public void SetObjectFactory(Func<Type, IEnumerable<InjectableMember>, CreationResult> factory)
        {
            this.createFunc = factory;
        }
    }
}