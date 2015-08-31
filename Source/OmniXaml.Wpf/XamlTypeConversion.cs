namespace OmniXaml.Wpf
{
    using System;
    using System.Linq;
    using System.Windows.Data;
    using System.Xaml;
    using Typing;
    using NamespaceDeclaration = NamespaceDeclaration;
    using XamlMember = System.Xaml.XamlMember;
    using XamlType = System.Xaml.XamlType;

    public static class XamlTypeConversion
    {
        public static System.Xaml.XamlNodeType ToWpf(this XamlInstructionType nodeType)
        {
            return (System.Xaml.XamlNodeType)Enum.Parse(typeof(XamlNodeType), nodeType.ToString());
        }

        public static System.Xaml.NamespaceDeclaration ToWpf(this NamespaceDeclaration namespaceDeclaration)
        {
            return new System.Xaml.NamespaceDeclaration(namespaceDeclaration.Namespace, namespaceDeclaration.Prefix);
        }

        public static XamlType ToWpf(this Typing.XamlType xamlType, XamlSchemaContext context)
        {
            if (typeof(IMarkupExtension).IsAssignableFrom(xamlType.UnderlyingType))
            {
                return new XamlType(typeof(Binding), context);
            }

            return new XamlType(xamlType.UnderlyingType, context);
        }

        public static XamlMember ToWpf(this XamlMemberBase member, XamlSchemaContext context)
        {
            if (member.IsDirective)
            {
                return GetDirective(member, context);
            }

            return GetMember((MutableXamlMember) member, context);
        }

        private static XamlMember GetMember(MutableXamlMember member, XamlSchemaContext context)
        {
            var declaringType = ToWpf(member.DeclaringType, context);

            if (!member.IsAttachable)
            {
                var xamlMember = declaringType.GetMember(member.Name);
                return new XamlMemberAdapter(xamlMember, context);
            }
            else
            {
                var xamlMember = declaringType.GetAttachableMember(member.Name);
                return new XamlMemberAdapter(xamlMember, context, member.Name, xamlMember.Invoker.UnderlyingGetter, xamlMember.Invoker.UnderlyingSetter);
            }
        }

        private static XamlMember GetDirective(XamlMemberBase directive, XamlSchemaContext context)
        {
            var directiveName = TranslateDirectiveName(directive);

            var xamlMember = from ns in context.GetAllXamlNamespaces()
                let dir = context.GetXamlDirective(ns, directiveName)
                where dir != null
                select dir;

            return new DirectiveAdapter(xamlMember.First());
        }

        private static string TranslateDirectiveName(XamlMemberBase member)
        {
            if (member.Name == "_MarkupExtensionParameters")
            {
                return "_PositionalParameters";
            }

            return member.Name;
        }
    }
}