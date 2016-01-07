namespace OmniXaml.Wpf
{
    using System;
    using System.Xaml;

    public static class XamlNodeExtensions
    {
        public static XamlNodeType ToWpf(this InstructionType omniNodeType)
        {
            switch (omniNodeType)
            {
                case InstructionType.StartObject:
                    return XamlNodeType.StartObject;
                case InstructionType.EndMember:
                    return XamlNodeType.EndMember;
                case InstructionType.EndObject:
                    return XamlNodeType.EndObject;
                case InstructionType.GetObject:
                    return XamlNodeType.GetObject;
                case InstructionType.NamespaceDeclaration:
                    return XamlNodeType.NamespaceDeclaration;
                case InstructionType.Value:
                    return XamlNodeType.Value;
                case InstructionType.None:
                    return XamlNodeType.None;
                case InstructionType.StartMember:
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

        public static XamlMember ToWpf(this Typing.Member omniMember, XamlSchemaContext xamlSchemaContext)
        {
            var declaringType = ToWpf(omniMember.DeclaringType, xamlSchemaContext);
            return declaringType.GetMember(omniMember.Name);
        }
    }
}