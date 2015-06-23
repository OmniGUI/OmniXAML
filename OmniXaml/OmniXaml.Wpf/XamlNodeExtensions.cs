namespace OmniXaml.Wpf
{
    using System;
    using System.Xaml;
    using XamlNodeType = XamlNodeType;

    public static class XamlNodeExtensions
    {
        public static System.Xaml.XamlNodeType ToWpf(this XamlNodeType omniNodeType)
        {
            switch (omniNodeType)
            {
                case XamlNodeType.StartObject:
                    return System.Xaml.XamlNodeType.StartObject;
                case XamlNodeType.EndMember:
                    return System.Xaml.XamlNodeType.EndMember;
                case XamlNodeType.EndObject:
                    return System.Xaml.XamlNodeType.EndObject;
                case XamlNodeType.GetObject:
                    return System.Xaml.XamlNodeType.GetObject;
                case XamlNodeType.NamespaceDeclaration:
                    return System.Xaml.XamlNodeType.NamespaceDeclaration;
                case XamlNodeType.Value:
                    return System.Xaml.XamlNodeType.Value;
                case XamlNodeType.None:
                    return System.Xaml.XamlNodeType.None;
                case XamlNodeType.StartMember:
                    return System.Xaml.XamlNodeType.StartMember;
            }

            throw new InvalidOperationException($"Cannot convert the value {omniNodeType} to a XamlNodeType for WPF.");
        }

        public static System.Xaml.NamespaceDeclaration ToWpf(this OmniXaml.NamespaceDeclaration omniNamespace)
        {
            return new NamespaceDeclaration(omniNamespace.Namespace, omniNamespace.Prefix);
        }

        public static XamlType ToWpf(this OmniXaml.Typing.XamlType omniType, XamlSchemaContext xamlSchemaContext)
        {
            return new XamlType(omniType.UnderlyingType, xamlSchemaContext);
        }

        public static XamlMember ToWpf(this OmniXaml.Typing.XamlMember omniMember, XamlSchemaContext xamlSchemaContext)
        {
            var declaringType = omniMember.DeclaringType.ToWpf(xamlSchemaContext);
            return declaringType.GetMember(omniMember.Name);
        }
    }
}