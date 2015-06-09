namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using BuiltInConverters;

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
            Register(typeof(bool), new BooleanConverter());
        }

        private void Register(Type type, ITypeConverter converter)
        {
            converters[type] = converter;
        }

        public ITypeConverter GetTypeConverter(Type type)
        {
            ITypeConverter converter;
            if (IsNullable(type))
            {
                type = Nullable.GetUnderlyingType(type);
            }
          
            return converters.TryGetValue(type, out converter) ? converter : null;
        }

        static bool IsNullable(Type type)
        {            
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}