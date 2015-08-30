namespace OmniXaml.Wpf
{
    using System;
    using System.Xaml;

    public static class XamlNodeExtensions
    {
        public static XamlNodeType ToWpf(this XamlInstructionType omniNodeType)
        {
            switch (omniNodeType)
            {
                case XamlInstructionType.StartObject:
                    return XamlNodeType.StartObject;
                case XamlInstructionType.EndMember:
                    return XamlNodeType.EndMember;
                case XamlInstructionType.EndObject:
                    return XamlNodeType.EndObject;
                case XamlInstructionType.GetObject:
                    return XamlNodeType.GetObject;
                case XamlInstructionType.NamespaceDeclaration:
                    return XamlNodeType.NamespaceDeclaration;
                case XamlInstructionType.Value:
                    return XamlNodeType.Value;
                case XamlInstructionType.None:
                    return XamlNodeType.None;
                case XamlInstructionType.StartMember:
                    return XamlNodeType.StartMember;
            }

            throw new InvalidOperationException($"Cannot convert the value {omniNodeType} to a XamlNodeType for WPF.");
        }

        public static NamespaceDeclaration ToWpf(this OmniXaml.NamespaceDeclaration omniNamespace)
        {
            return new NamespaceDeclaration(omniNamespace.Namespace, omniNamespace.Prefix);
        }

        public static XamlType ToWpf(this Typing.XamlType omniType, XamlSchemaContext xamlSchemaContext)
        {
            return new XamlType(omniType.UnderlyingType, xamlSchemaContext);
        }

        public static XamlMember ToWpf(this Typing.XamlMember omniMember, XamlSchemaContext xamlSchemaContext)
        {
            var declaringType = ToWpf(omniMember.DeclaringType, xamlSchemaContext);
            return declaringType.GetMember(omniMember.Name);
        }
    }
}