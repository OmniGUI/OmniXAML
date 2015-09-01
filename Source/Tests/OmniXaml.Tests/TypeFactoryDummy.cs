namespace OmniXaml.Tests
{
    using System;

    public class TypeFactoryDummy : ITypeFactory
    {
        public object Create(Type type, object[] args)
        {
            throw new NotImplementedException();
        }

        public bool CanCreate(Type type)
        {
            throw new NotImplementedException();
        }
    }
}