namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using Glass;

    public class TypeFactory : ITypeFactory
    {
        public object Create(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return Create(type, null);
        }

        public object Create(Type type, object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        public bool CanLocate(Type type)
        {
            return true;
        }

        public IList<Type> LookupUserInjectableParameters(Type type, int parameterCount)
        {
            throw new NotImplementedException();
        }
    }
}