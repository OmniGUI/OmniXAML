namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Builder;
    using Glass;

    public class RuntimeNamePropertyProvider : IRuntimeNameProvider
    {
        private readonly IDictionary<Type, RuntimeNamePropertyRegistration> registrations = new Dictionary<Type, RuntimeNamePropertyRegistration>();

        private void Register(Type type, string nameProperty)
        {
            registrations[type] = new RuntimeNamePropertyRegistration(type, nameProperty);
        }

        public string GetRuntimeNameProperty(Type type)
        {
            RuntimeNamePropertyRegistration member;
            var explictRegistration = registrations.TryGetValue(type, out member) ? member : null;

            if (explictRegistration == null)
            {
                return ResolveFromHierarchy(type);
            }

            return explictRegistration.Name;
        }

        private string ResolveFromHierarchy(Type type)
        {
            while (true)
            {
                if (type == null)
                {
                    return null;
                }

                RuntimeNamePropertyRegistration member;
                var baseRegistration = registrations.TryGetValue(type, out member) ? member : null;
                if (baseRegistration != null)
                {
                    return baseRegistration.Name;
                }

                var typeToLookFor = type.GetTypeInfo().BaseType;
                type = typeToLookFor;
            }
        }

        public static IRuntimeNameProvider FromAttributes(IEnumerable<Type> types)
        {
            var contentPropertyProvider = new RuntimeNamePropertyProvider();

            var defs = Extensions.GatherAttributes<RuntimeNamePropertyAttribute, RuntimeNamePropertyRegistration>(
                types,
                (type, attribute) => new RuntimeNamePropertyRegistration(type, attribute.Name));

            contentPropertyProvider.AddAll(defs);
            return contentPropertyProvider;
        }

        public IEnumerator<RuntimeNamePropertyRegistration> GetEnumerator()
        {
            var typeConverterRegistrations = registrations.Select(pair => new RuntimeNamePropertyRegistration(pair.Key, GetRuntimeNameProperty(pair.Key)));
            return typeConverterRegistrations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(RuntimeNamePropertyRegistration item)
        {
            registrations.Add(item.Type, item);
        }        
    }

    public class RuntimeNamePropertyRegistration
    {
        public RuntimeNamePropertyRegistration(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
        public string RuntimeName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class RuntimeNamePropertyAttribute : Attribute
    {
        public string Name { get; set; }
    }
}