namespace OmniXaml.AppServices
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    public class InflatableTypeFactory : ITypeFactory
    {
        public ILoader Loader { get; set; }

        private readonly ITypeFactory coreTypeFactory;
        private readonly IResourceProvider resourceProvider;
        private readonly ITypeToUriLocator typeToUriLocator;
        public IEnumerable<Type> Inflatables { get; set; } = new Collection<Type>();

        public InflatableTypeFactory(ITypeFactory coreTypeFactory, IResourceProvider resourceProvider, ITypeToUriLocator typeToUriLocator)
        {
            this.coreTypeFactory = coreTypeFactory;
            this.resourceProvider = resourceProvider;
            this.typeToUriLocator = typeToUriLocator;
        }

        public T Create<T>()
        {
            return (T) Create(typeof (T));
        }

        private bool IsInflatable(Type type) => Inflatables.Any(t => t.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()));

        public object CreateWithInflate(Type type, Uri uri) 
        {
            using (var stream = resourceProvider.GetStream(uri))
            {
                var instance = coreTypeFactory.Create(type);
                var window = Loader.Load(stream, instance);
                return window;
            }
        }

        public object Create(Type type)
        {
            if (IsInflatable(type))
            {
                var uri = typeToUriLocator.GetUriFor(type);
                return CreateWithInflate(type, uri);
            }

            return coreTypeFactory.Create(type);
        }

        public object Create(Type type, object[] args)
        {
            throw new NotImplementedException();
        }

        public bool CanLocate(Type type)
        {
            return coreTypeFactory.CanLocate(type);
        }

        public IList<Type> LookupUserInjectableParameters(Type type, int parameterCount)
        {
            throw new NotImplementedException();
        }

        public object Create(Uri uri)
        {
            var type = typeToUriLocator.GetTypeFor(uri);
            return Create(type);
        }
    }
}