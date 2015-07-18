namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using AppServices;
    using AppServices.NetCore;

    public class ObjectFactory : ITypeFactory
    {
        private readonly InflatableTypeFactory inflater;

        public ObjectFactory()
        {

            inflater = new InflatableTypeFactory(new TypeFactory(), new NetCoreResourceProvider(), new NetCoreTypeToUriLocator());
            inflater.XamlStreamLoaderFactoryMethod = _ => new WpfXamlStreamLoader();
            inflater.Inflatables = new Collection<Type> { typeof(Window) };
        }

        public object Create(Type type)
        {
            return inflater.Create(type);
        }

        public object Create(Type type, object[] args)
        {
            return inflater.Create(type, args);
        }

        public bool CanLocate(Type type)
        {
            return inflater.CanLocate(type);
        }

        public IList<Type> LookupUserInjectableParameters(Type type, int parameterCount)
        {
            throw new NotImplementedException();
        }

        public T Create<T>()
        {
            return (T)inflater.Create(typeof(T));
        }
    }
}
