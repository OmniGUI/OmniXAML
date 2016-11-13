namespace Glass.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Utils
    {
        public static void UniversalAdd(object collection, object item)
        {
            var colType = collection.GetType();

            var addMethod = colType.GetTypeInfo().ImplementedInterfaces.SelectMany(x => x.GetRuntimeMethods()).First(n => n.Name == "Add");

            if (addMethod == null)
            {
                throw new InvalidOperationException($"The type {colType} is not a dictionary");
            }

            if (addMethod.GetParameters().Length == 1)
            {
                ParameterInfo parameter = addMethod.GetParameters().First();
                if (parameter.ParameterType.GetTypeInfo().IsAssignableFrom(item.GetType().GetTypeInfo()))
                {
                    addMethod.Invoke(collection, new[] {item});
                }
                else
                {
                    throw new InvalidOperationException($"The item {item} is not assignable to the dictionary {collection}. The types are incompatible.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Cannot find an appropriate method to add the {item} to {collection}.");
            }
        }

        public static void UniversalAddToDictionary(object dictionary, object item, object key)
        {
            var dictType = dictionary.GetType();

            var addMethod = dictType.GetTypeInfo().ImplementedInterfaces.SelectMany(x => x.GetRuntimeMethods()).First(n => n.Name == "Add");

            if (addMethod == null)
            {
                throw new InvalidOperationException($"The type {dictType} is not a dictionary");
            }

            var parameterInfos = addMethod.GetParameters().ToList();

            if (parameterInfos.Count == 2)
            {
                var keyParameter = parameterInfos[0];

                if (!keyParameter.ParameterType.GetTypeInfo().IsAssignableFrom(key.GetType().GetTypeInfo()))
                {
                    throw new InvalidOperationException($"The keys are incompatible. {key} is not assignable to the key of {dictionary} (which its type is {dictType}.");
                }

                ParameterInfo itemParameter = parameterInfos[1];

                if (itemParameter.ParameterType.GetTypeInfo().IsAssignableFrom(item.GetType().GetTypeInfo()))
                {
                    addMethod.Invoke(dictionary, new[] { key, item});
                }
                else
                {
                    throw new InvalidOperationException($"The item {item} is not assignable to the dictionary {dictionary}. The types are incompatible.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Cannot find an appropriate method to add the {item} to {dictionary}.");
            }
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