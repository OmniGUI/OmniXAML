namespace OmniXaml.Typing
{
    using System;
    using System.Reflection;

    public interface IXamlTypeRepository
    {
        XamlType Get(Type type);
        XamlType GetByPrefix(string prefix, string typeName);
        XamlType GetWithFullAddress(XamlTypeName xamlTypeName);
        XamlMember GetMember(PropertyInfo propertyInfo);
    }
}