namespace OmniXaml.Services.Tests
{
    using System;
    using OmniXaml.Tests.Classes;

    internal class TypeFactoryMock : ITypeFactory
    {
        private readonly Func<object, object[], ChildClass> func;

        public TypeFactoryMock(Func<object, object[], ChildClass> func)
        {
            this.func = func;
        }

        public object Create(Type type, params object[] args)
        {
            return func(type, args);
        }

        public bool CanCreate(Type type)
        {
            return true;
        }
    }
}