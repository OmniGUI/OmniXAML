namespace OmniXaml
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Glass;
    using Parsers.ProtoParser;
    using Typing;

    public class ProtoNodeBuilder
    {
        private readonly ITypeContext typeContext;
        private readonly ITypeFeatureProvider typeFeatureProvider;
        private readonly ITypeFactory typeFactory;

        public ProtoNodeBuilder(ITypeContext typeContext, ITypeFeatureProvider typeFeatureProvider)
        {
            this.typeContext = typeContext;
            this.typeFeatureProvider = typeFeatureProvider;
            this.typeFactory = typeContext.TypeFactory;
        }

        public PrefixRegistrationMode PrefixRegistrationMode { get; set; } = PrefixRegistrationMode.Automatic;

        public ProtoXamlNode None()
        {
            return new ProtoXamlNode
            {
                Namespace = null,
                XamlType = null,
                NodeType = NodeType.None
            };
        }

        public ProtoXamlNode NamespacePrefixDeclaration(NamespaceDeclaration ns)
        {
            return NamespacePrefixDeclaration(ns.Prefix, ns.Namespace);
        }

        public ProtoXamlNode NamespacePrefixDeclaration(string prefix, string ns)
        {
            if (PrefixRegistrationMode == PrefixRegistrationMode.Automatic)
            {
                var prefixRegistration = new PrefixRegistration(prefix, ns);
                if (!typeContext.RegisteredPrefixes.Contains(prefixRegistration))
                {
                    typeContext.RegisterPrefix(prefixRegistration);
                }
            }

            return new ProtoXamlNode
            {
                Namespace = ns,
                XamlType = null,
                NodeType = NodeType.PrefixDefinition,
                Prefix = prefix,
            };
        }

        private ProtoXamlNode Element(Type type, NamespaceDeclaration nsDecl, bool isEmpty)
        {
            return new ProtoXamlNode
            {
                Namespace = nsDecl.Namespace,
                Prefix = nsDecl.Prefix,
                XamlType = XamlType.Create(type, typeContext, typeFactory, typeFeatureProvider),
                NodeType = isEmpty ? NodeType.EmptyElement : NodeType.Element,
            };
        }

        public ProtoXamlNode NonEmptyElement<T>(NamespaceDeclaration nsDecl = null)
        {
            return Element(typeof(T), nsDecl, false);
        }

        public ProtoXamlNode NonEmptyElement(Type type, NamespaceDeclaration nsDecl = null)
        {
            return Element(type, nsDecl, false);
        }

        public ProtoXamlNode EmptyElement(Type type, NamespaceDeclaration nsDecl)
        {
            return Element(type, nsDecl, true);
        }

        internal ProtoXamlNode EmptyElement<T>()
        {
            return EmptyElement<T>(null);
        }

        internal ProtoXamlNode EmptyElement<T>(NamespaceDeclaration namespaceDeclaration)
        {
            return EmptyElement(typeof(T), namespaceDeclaration);
        }

        public ProtoXamlNode AttachableProperty<TParent>(string name, string value, NamespaceDeclaration namespaceDeclaration)
        {
            var type = typeof(TParent);
            var xamlType = typeContext.GetXamlType(type);

            var member = xamlType.GetAttachableMember(name);

            return new ProtoXamlNode
            {
                Namespace = null,
                NodeType = NodeType.Attribute,
                XamlType = null,
                PropertyAttribute = member,
                PropertyAttributeText = value,
                Prefix = namespaceDeclaration.Prefix,
            };
        }

        // ReSharper disable once UnusedMember.Global
        public ProtoXamlNode EmptyPropertyElement<T>(Expression<Func<T, object>> selector, NamespaceDeclaration namespaceDeclaration)
        {
            return PropertyElement(selector, namespaceDeclaration, isCollapsed: true);
        }

        public ProtoXamlNode NonEmptyPropertyElement<T>(Expression<Func<T, object>> selector, NamespaceDeclaration namespaceDeclaration = null)
        {
            return PropertyElement(selector, namespaceDeclaration, isCollapsed: false);
        }

        public ProtoXamlNode NonEmptyPropertyElement(Type type, string memberName, NamespaceDeclaration namespaceDeclaration)
        {
            return PropertyElement(type, memberName, namespaceDeclaration, isCollapsed: false);
        }

        private ProtoXamlNode PropertyElement(Type type, string memberName, NamespaceDeclaration namespaceDeclaration, bool isCollapsed)
        {
            var property = typeContext.GetXamlType(type).GetMember(memberName);

            return new ProtoXamlNode
            {
                PropertyElement = property,
                Prefix = namespaceDeclaration.Prefix,
                Namespace = namespaceDeclaration.Namespace,
                XamlType = null,
                NodeType =
                    isCollapsed
                        ? NodeType.EmptyPropertyElement
                        : NodeType.PropertyElement
            };
        }

        private ProtoXamlNode PropertyElement<T>(Expression<Func<T, object>> selector, NamespaceDeclaration namespaceDeclaration, bool isCollapsed)
        {
            return PropertyElement(typeof(T), selector.GetFullPropertyName(), namespaceDeclaration, isCollapsed);
        }

        public ProtoXamlNode EndTag()
        {
            return new ProtoXamlNode
            {
                Namespace = null,
                XamlType = null,
                NodeType = NodeType.EndTag
            };
        }

        public ProtoXamlNode Text(string text = null)
        {
            return new ProtoXamlNode { Namespace = null, NodeType = NodeType.Text, XamlType = null, Text = text };
        }

        public ProtoXamlNode Attribute(XamlMemberBase member, string value, NamespaceDeclaration namespaceDeclaration)
        {
            return new ProtoXamlNode
            {
                PropertyAttribute = member,
                NodeType = NodeType.Attribute,
                PropertyAttributeText = value,
                Prefix = namespaceDeclaration.Prefix,
            };
        }

        public ProtoXamlNode Attribute<T>(Expression<Func<T, object>> selector, string value, NamespaceDeclaration namespaceDeclaration)
        {
            var xamlMember = typeContext.GetXamlType(typeof(T)).GetMember(selector.GetFullPropertyName());
            return Attribute(xamlMember, value, namespaceDeclaration);
        }

        public ProtoXamlNode Key(string value)
        {
            return Attribute(CoreTypes.Key, value, new NamespaceDeclaration("http://schemas.microsoft.com/winfx/2006/xaml", "x"));
        }
    }
}