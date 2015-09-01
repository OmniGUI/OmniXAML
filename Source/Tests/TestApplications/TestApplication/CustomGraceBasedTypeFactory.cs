namespace SampleWpfApp
{
    using System;
    using Grace.DependencyInjection;
    using OmniXaml;
    using OmniXaml.Services.Mvvm;

    public class CustomGraceBasedTypeFactory : ITypeFactory
    {
        private readonly IDependencyInjectionContainer container;
        private readonly ITypeFactory defaultTypeFactory = new TypeFactory();

        public CustomGraceBasedTypeFactory(IDependencyInjectionContainer container)
        {
            this.container = container;
        }

        public object Create(Type type, object[] args)
        {
            if (typeof(ViewModel).IsAssignableFrom(type))
            {
                return container.Locate(type);
            }

            return defaultTypeFactory.Create(type, args);
        }

        public bool CanCreate(Type type)
        {
            return true;
        }
    }
}