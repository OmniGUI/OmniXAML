namespace OmniXaml.Tests.Rework
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Subjects;

    internal class InstanceCreatorMock : IInstanceCreator
    {
        public ISubject<object> InstanceCreated { get; } = new Subject<object>();

        public object Create(Type type, IBuildContext context, IEnumerable<InjectableMember> injectableMembers = null)
        {
            var instance = Activator.CreateInstance(type);
            InstanceCreated.OnNext(instance);
            return instance;
        }
    }
}