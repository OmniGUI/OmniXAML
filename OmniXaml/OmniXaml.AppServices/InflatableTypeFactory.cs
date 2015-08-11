namespace OmniXaml.AppServices
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    public class InflatableTypeFactory : ITypeFactory
    {
        private readonly ITypeFactory coreTypeFactory;
        private readonly IInflatableTranslator inflatableTranslator;
        private readonly Func<InflatableTypeFactory, IXamlLoader> loaderFactory;

        public IEnumerable<Type> Inflatables { get; set; } = new Collection<Type>();

        public InflatableTypeFactory(ITypeFactory coreTypeFactory,
            IInflatableTranslator inflatableTranslator,
            Func<InflatableTypeFactory, IXamlLoader> loaderFactory)
        {
            this.coreTypeFactory = coreTypeFactory;
            this.inflatableTranslator = inflatableTranslator;
            this.loaderFactory = loaderFactory;
        }

        public T Create<T>()
        {
            return (T) Create(typeof (T));
        }

        private bool IsInflatable(Type type) => HasSomeInflatableThatIsCompatible(type);

        private bool HasSomeInflatableThatIsCompatible(Type type)
        {
            return Inflatables.Any(t => t.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()));
        }

        public object Create(Type type)
        {
            return Create(type, null);
        }

        public object Create(Type type, object[] args)
        {
            if (IsInflatable(type))
            {                
                using (var stream = inflatableTranslator.GetStream(type))
                {
                    var instance = coreTypeFactory.Create(type, args);
                    var loader = loaderFactory(this);
                    var inflated = loader.Load(stream, instance);
                    return inflated;
                }
            }

            return coreTypeFactory.Create(type, args);
        }

        public bool CanLocate(Type type)
        {
            return coreTypeFactory.CanLocate(type);
        }

        public object Create(Uri uri)
        {
            var type = inflatableTranslator.GetTypeFor(uri);
            return Create(type);
        }
    }
}