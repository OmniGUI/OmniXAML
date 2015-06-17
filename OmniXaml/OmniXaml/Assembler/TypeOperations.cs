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
            return property.XamlMemberValueConnector.GetValue(parentInstance);            
        }

        public static void SetValue(object instance, XamlMember parentProperty, object value)
        {
            parentProperty.XamlMemberValueConnector.SetValue(instance, value);          
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