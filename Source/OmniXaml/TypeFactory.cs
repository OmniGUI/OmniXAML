namespace OmniXaml
{
    using System;

    public class TypeFactory : ITypeFactory
    {
        public object Create(Type type, object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        public bool CanCreate(Type type)
        {
            return true;
        }
    }
}