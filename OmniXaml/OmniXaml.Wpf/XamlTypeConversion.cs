namespace OmniXaml.Wpf
{
    using System;
    using System.Windows.Data;
    using System.Xaml;
    using NamespaceDeclaration = OmniXaml.NamespaceDeclaration;
    using XamlNodeType = OmniXaml.XamlNodeType;

    public static class XamlTypeConversion
    {
        public static System.Xaml.XamlNodeType ToWpf(this XamlNodeType nodeType)
        {
            return (System.Xaml.XamlNodeType) Enum.Parse(typeof (System.Xaml.XamlNodeType), nodeType.ToString());
        }

        public static System.Xaml.NamespaceDeclaration ToWpf(this NamespaceDeclaration namespaceDeclaration)
        {
            return new System.Xaml.NamespaceDeclaration(namespaceDeclaration.Namespace, namespaceDeclaration.Prefix);
        }

        public static XamlType ToWpf(this Typing.XamlType xamlType, XamlSchemaContext context)
        {
            if (typeof (IMarkupExtension).IsAssignableFrom(xamlType.UnderlyingType))
            {
                return new MarkupExtensionXamlType(typeof (Binding), context);
            }

            return new XamlType(xamlType.UnderlyingType, context);
        }

        public static XamlMember ToWpf(this Typing.XamlMember member, XamlSchemaContext context)
        {
            if (!member.IsDirective)
            {
                var declaringType = ToWpf(member.DeclaringType, context);

                var xamlMember = member.IsAttachable == false ? declaringType.GetMember(member.Name) : declaringType.GetAttachableMember(member.Name);
                return new NastyXamlMember(xamlMember, context);
            }
            else
            {
                var xamlMember = context.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", member.Name);
                var nastyDirective = new NastyDirective(xamlMember);
                return nastyDirective;
            }
        }
    }

    public class NastyDirective : XamlDirective
    {
        public NastyDirective(XamlDirective xamlMember)
            : base(xamlMember.GetXamlNamespaces(), xamlMember.Name, xamlMember.Type, xamlMember.TypeConverter, xamlMember.AllowedLocation)
        {
        }
    }
}