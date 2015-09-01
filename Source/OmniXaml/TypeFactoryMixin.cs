namespace OmniXaml
{
    using System;

    public static class TypeFactoryMixin
    {
        public static object Create(this ITypeFactory typeFactory, Type type)
        {
            return typeFactory.Create(type, null);
        }

        public static T Create<T>(this ITypeFactory typeFactory)
        {
            return (T)typeFactory.Create(typeof (T));
        }

        public static T Create<T>(this ITypeFactory typeFactory, params object[] args)
        {
            return (T)typeFactory.Create(typeof(T), args);
        }
    }
}