namespace OmniXaml.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    public class AutoInflatingTypeFactory : ITypeFactory
    {
        private readonly ITypeFactory innerTypeFactory;
        private readonly IInflatableTranslator inflatableTranslator;
        private readonly Func<ITypeFactory, IXamlLoader> xamlLoaderFactory;

        public virtual IEnumerable<Type> Inflatables => new Collection<Type>();

        public AutoInflatingTypeFactory(ITypeFactory innerTypeFactory,
            IInflatableTranslator inflatableTranslator,
            Func<ITypeFactory, IXamlLoader> xamlLoaderFactory)
        {
            this.innerTypeFactory = innerTypeFactory;
            this.inflatableTranslator = inflatableTranslator;
            this.xamlLoaderFactory = xamlLoaderFactory;
        }

        public object Create(Type type, object[] args)
        {
            return IsInflatable(type) ? CreateAndInflate(type, args) : CreateWithNoInflate(type, args);
        }

        private object CreateWithNoInflate(Type type, object[] args)
        {
            return innerTypeFactory.Create(type, args);
        }

        private object CreateAndInflate(Type type, object[] args)
        {
            using (var stream = inflatableTranslator.GetInflationSourceStream(type))
            {
                var instance = innerTypeFactory.Create(type, args);
                var loader = xamlLoaderFactory(this);
                var inflated = loader.Load(stream, instance);
                return inflated;
            }
        }

        private bool IsInflatable(Type type) => HasSomeInflatableThatIsCompatible(type);

        private bool HasSomeInflatableThatIsCompatible(Type type)
        {
            return Inflatables.Any(t => t.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()));
        }

        public bool CanCreate(Type type)
        {
            return innerTypeFactory.CanCreate(type);
        }

        public object Create(Uri uri)
        {
            var type = inflatableTranslator.GetTypeFor(uri);
            return this.Create(type);
        }
    }
}