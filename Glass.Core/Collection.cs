namespace Glass.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Collection
    {
        private const string AddMethodName = "Add";

        public static void UniversalAdd(object collection, object item)
        {
            var colType = collection.GetType();

            var addMethod = MethodFinder.GetAddMethod(AddMethodName, colType, item.GetType());

            if (addMethod == null)
            {
                throw new InvalidOperationException($"Cannot find an appropriate method to add the {item} to {collection}.");
            }

            addMethod.Invoke(collection, new[] { item });
        }

        public static void UniversalAddToDictionary(object dictionary, object item, object key)
        {
            var dictType = dictionary.GetType();

            var addMethod = MethodFinder.GetAddMethod(AddMethodName, dictType, key.GetType(), item.GetType());

            if (addMethod == null)
            {
                throw new InvalidOperationException($"Cannot find an appropriate method to add the {item} to {dictionary}.");
            }

            addMethod.Invoke(dictionary, new[] { key, item });
        }

        public static bool ContentEquals<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> otherDictionary)
        {
            return (otherDictionary ?? new Dictionary<TKey, TValue>())
                .OrderBy(kvp => kvp.Key)
                .SequenceEqual((dictionary ?? new Dictionary<TKey, TValue>())
                                   .OrderBy(kvp => kvp.Key));
        }
    }
}