namespace OmniXaml.Tests
{
    using System;
    using System.Collections.Generic;

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

        public IList<Type> LookupUserInjectableParameters(Type type, int parameterCount)
        {
            throw new NotImplementedException();
        }
    }
}