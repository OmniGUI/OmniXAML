namespace OmniXaml.Typing
{
    using System;
    using System.Reflection;

    public interface ITypeRepository
    {
        XamlType GetByType(Type type);
        XamlType GetByQualifiedName(string qualifiedName);
        XamlType GetByPrefix(string prefix, string typeName);
        XamlType GetByFullAddress(XamlTypeName xamlTypeName);
        Member GetMember(PropertyInfo propertyInfo);
        AttachableMember GetAttachableMember(string name, MethodInfo getter, MethodInfo setter);        
    }
}