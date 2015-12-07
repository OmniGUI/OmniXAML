namespace OmniXaml.Services
{
    using System;

    public class TypeFactoryRegistration
    {
        public ITypeFactory Factory { get; }
        public Func<Type, bool> IsApplicable { get; }

        public TypeFactoryRegistration(ITypeFactory factory, Func<Type, bool> isApplicable)
        {
            this.Factory = factory;
            this.IsApplicable = isApplicable;
        }
    }
}