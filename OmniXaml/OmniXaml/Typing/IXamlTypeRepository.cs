namespace OmniXaml.Typing
{
    using System;

    public interface IXamlTypeRepository
    {
        XamlType Get(Type type);
        XamlType GetByPrefix(string prefix, string typeName);
        XamlType GetWithFullAddress(XamlTypeName xamlTypeName);
    }
}