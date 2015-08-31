namespace SampleWpfApp
{
    using System;
    using Grace.DependencyInjection;
    using OmniXaml;
    using OmniXaml.AppServices.Mvvm;

    public class CustomGraceBasedTypeFactory : ITypeFactory
    {
        private readonly IDependencyInjectionContainer container;
        private readonly ITypeFactory defaultTypeFactory = new TypeFactory();

        public CustomGraceBasedTypeFactory(IDependencyInjectionContainer container)
        {
            this.container = container;
        }

        public object Create(Type type)
        {
            return container.Locate(type);
        }

        public object Create(Type type, object[] args)
        {
            if (typeof(ViewModel).IsAssignableFrom(type))
            {
                return Create(type);
            }

            return defaultTypeFactory.Create(type, args);
        }

        public bool CanLocate(Type type)
        {
            return true;
        }
    }
}