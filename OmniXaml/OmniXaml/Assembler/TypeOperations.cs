namespace OmniXaml.Assembler
{
    using System.Collections;
    using System.Reflection;
    using Typing;

    public class TypeOperations
    {
        private readonly ITypeFactory typeFactory;

        public TypeOperations(ITypeFactory typeFactory)
        {
            this.typeFactory = typeFactory;
        }

        public static void Add(object collection, object instance)
        {
            var list = (IList)collection;
            list.Add(instance);
        }

        public static object GetValue(object parentInstance, XamlMember property)
        {
            var prop = parentInstance.GetType().GetRuntimeProperty(property.Name);
            return prop.GetValue(parentInstance);
        }

        public static void SetValue(object instance, XamlMember parentProperty, object value)
        {
            if (parentProperty.IsAttachable)
            {
                var underlyingType = parentProperty.DeclaringType.UnderlyingType;
                var prop = underlyingType.GetTypeInfo().GetDeclaredMethod("Set" + parentProperty.Name);                    
                prop.Invoke(null, new[] { instance, value});
            }
            else
            {
                var prop = instance.GetType().GetRuntimeProperty(parentProperty.Name);
                prop.SetValue(instance, value);
            }
        }

        public object Create(XamlType xamlType)
        {
            if (xamlType.IsUnreachable)
            {
                throw new XamlReaderException($"The underlying type is null for this XAML Type: {xamlType}");
            }

            return typeFactory.Create(xamlType.UnderlyingType);            
        }
    }
}