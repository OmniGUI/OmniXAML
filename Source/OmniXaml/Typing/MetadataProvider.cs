namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Glass.Core;

    public class MetadataProvider
    {
        private readonly IDictionary<string, AutoKeyDictionary<Type, object>> lookupDictionaries = new Dictionary<string, AutoKeyDictionary<Type, object>>();

        public void Register(Type type, Metadata medataData)
        {
            foreach (var propertyInfo in medataData.GetType().GetRuntimeProperties())
            {
                var dic = GetDictionary(propertyInfo.Name);
                var value = propertyInfo.GetValue(medataData);
                if (value != null)
                {
                    dic.Add(type, value);
                }
            }
        }

        private AutoKeyDictionary<Type, object> GetDictionary(string name)
        {
            AutoKeyDictionary<Type, object> dic;
            var hadValue = lookupDictionaries.TryGetValue(name, out dic);
            if (hadValue)
            {
                return dic;
            }

            var autoKeyDictionary = new AutoKeyDictionary<Type, object>(t => t.GetTypeInfo().BaseType, t => t != null);
            lookupDictionaries.Add(name, autoKeyDictionary);
            return autoKeyDictionary;
        }

        public Metadata Get(Type type)
        {
            var metadata = new Metadata();
            foreach (var prop in MedataProperties)
            {
                AutoKeyDictionary<Type, object> dic;
                var hadDictionary  = lookupDictionaries.TryGetValue(prop.Name, out dic);
                if (hadDictionary)
                {
                    object value;
                    var hadValue = dic.TryGetValue(type, out value);
                    if (hadValue)
                    {
                        prop.SetValue(metadata, value);
                    }
                    else
                    {
                        prop.SetValue(metadata, null);
                    }
                }
                else
                {
                    prop.SetValue(metadata, null);
                }
            }

            return metadata;
        }

        private static IEnumerable<PropertyInfo> MedataProperties => typeof(Metadata).GetRuntimeProperties();
    }
}