namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections.Generic;

    public class TypeConverterProvider : ITypeConverterProvider
    {
        readonly IDictionary<Type, ITypeConverter> converters = new Dictionary<Type, ITypeConverter>();

        public TypeConverterProvider()
        {
            RegisterBuiltIn();
        }

        private void RegisterBuiltIn()
        {
            Register(typeof(string), new StringTypeConverter());
            Register(typeof(int), new NumberTypeConverter());
            Register(typeof(long), new NumberTypeConverter());
            Register(typeof(short), new NumberTypeConverter());
            Register(typeof(double), new NumberTypeConverter());
            Register(typeof(float), new NumberTypeConverter());
        }

        private void Register(Type type, ITypeConverter converter)
        {
            converters[type] = converter;
        }

        public void AddCatalog(IDictionary<Type, ITypeConverter> catalog)
        {
            foreach (var typeConverter in catalog)
            {
                converters.Add(typeConverter);
            }
        }

        public ITypeConverter GetTypeConverter(Type getType)
        {
            ITypeConverter converter;
            return converters.TryGetValue(getType, out converter) ? converter : null;
        }
    }
}