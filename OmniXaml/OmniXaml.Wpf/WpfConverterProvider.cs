namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using TypeConversion;
    using TypeConverterAttribute = System.ComponentModel.TypeConverterAttribute;

    internal class WpfConverterProvider : ITypeConverterProvider
    {
        private readonly TypeConverterProvider fallback;

        public WpfConverterProvider()
        {
            fallback = new TypeConverterProvider();
        }

        public ITypeConverter GetTypeConverter(Type type)
        {
            var converter = fallback.GetTypeConverter(type);
            if (converter == null)
            {
                var typeConverterAttribute = type.GetTypeInfo().GetCustomAttribute<TypeConverterAttribute>();

                if (typeConverterAttribute == null)
                {
                    return null;
                }

                var qualifiedName = typeConverterAttribute.ConverterTypeName;
                var converterType = Type.GetType(qualifiedName, true);
                var converterInstance = (TypeConverter) Activator.CreateInstance(converterType);
                converter = new ConverterAdapter(converterInstance);
            }
            return converter;
        }

        public void AddCatalog(IDictionary<Type, ITypeConverter> typeConverters)
        {
            throw new NotImplementedException();
        }
    }
}