namespace OmniXaml.Typing
{
    using System;
    using System.Reflection;

    public interface IXamlTypeRepository
    {
        XamlType GetXamlType(Type type);
        XamlType GetByPrefix(string prefix, string typeName);
        XamlType GetWithFullAddress(XamlTypeName xamlTypeName);
        XamlMember GetMember(PropertyInfo propertyInfo);
    }
}