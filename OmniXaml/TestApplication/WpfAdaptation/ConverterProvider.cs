namespace TestApplication.WpfAdaptation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using OmniXaml.TypeConversion;
    using TypeConverterAttribute = System.ComponentModel.TypeConverterAttribute;

    public class ConverterProvider : ITypeConverterProvider
    {
        private readonly TypeConverterProvider fallback;

        public ConverterProvider()
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