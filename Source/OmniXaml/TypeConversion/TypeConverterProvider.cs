namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Builder;
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
            Register(typeof(int), new IntTypeConverter());
            Register(typeof(long), new IntTypeConverter());
            Register(typeof(short), new IntTypeConverter());
            Register(typeof(double), new DoubleTypeConverter());
            Register(typeof(float), new IntTypeConverter());
            Register(typeof(bool), new BooleanConverter());
            Register(typeof(Type), new TypeTypeConverter());
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



        public IEnumerator<TypeConverterRegistration> GetEnumerator()
        {
            var typeConverterRegistrations = converters.Select(pair => new TypeConverterRegistration(pair.Key, GetTypeConverter(pair.Key)));
            return typeConverterRegistrations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TypeConverterRegistration item)
        {
            converters.Add(item.TargetType, item.TypeConverter);
        }        
    }
}