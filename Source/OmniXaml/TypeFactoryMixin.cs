namespace OmniXaml
{
    public static class TypeFactoryMixin
    {
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