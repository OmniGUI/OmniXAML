namespace OmniXaml.Tests
{
    using System;

    public class TypeFactoryDummy : ITypeFactory
    {
        public object Create(Type type)
        {
            throw new NotImplementedException();
        }

        public object Create(Type type, object[] args)
        {
            throw new NotImplementedException();
        }

        public bool CanLocate(Type type)
        {
            throw new NotImplementedException();
        }
    }
}