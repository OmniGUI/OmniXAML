namespace OmniXaml.ObjectAssembler
{
    using System.Collections;

    public class TypeOperations
    {
        private readonly ITypeFactory typeFactory;

        public TypeOperations(ITypeFactory typeFactory)
        {
            this.typeFactory = typeFactory;
        }

        public static void AddToCollection(ICollection collection, object instance)
        {
            ((IList)collection).Add(instance);
        }

        public static void AddToDictionary(IDictionary collection, object key, object value)
        {
            collection.Add(key, value);
        }
    }
}