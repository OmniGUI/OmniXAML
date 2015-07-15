namespace OmniXaml.AppServices
{
    using System;

    public class XamlWindowFactory : IWindowFactory
    {
        private readonly IXamlLoader xamlLoader;

        private readonly ITypeFactory typeFactory;
        private readonly IResourceProvider resourceProvider;
        private readonly IXamlByTypeProvider xamlByTypeProvider;

        public XamlWindowFactory(IXamlLoader xamlLoader, ITypeFactory typeFactory, IResourceProvider resourceProvider, IXamlByTypeProvider xamlByTypeProvider)
        {
            this.xamlLoader = xamlLoader;
            this.typeFactory = typeFactory;
            this.resourceProvider = resourceProvider;
            this.xamlByTypeProvider = xamlByTypeProvider;
        }

        public T Create<T>()
        {
            var uri = xamlByTypeProvider.GetUriFor(typeof (T));
            return (T) Create(typeof (T), uri);
        }

        public T Create<T>(Uri uri)
        {
            return (T)Create(typeof(T), uri);
        }

        public object Create(Type type, Uri uri) 
        {
            using (var stream = resourceProvider.GetStream(uri))
            {
                var instance = typeFactory.Create(type);
                var window = xamlLoader.Load(stream, instance);
                return window;
            }
        }
    }
}