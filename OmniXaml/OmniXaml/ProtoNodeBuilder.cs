namespace OmniXaml
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Glass;
    using Parsers.ProtoParser;
    using Typing;

    internal class ProtoNodeBuilder
    {
        private readonly ITypeContext typeContext;

        public ProtoNodeBuilder(ITypeContext typeContext)
        {
            this.typeContext = typeContext;
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

        private ProtoXamlNode Element(Type type, string prefix, bool isEmpty)
        {
            return new ProtoXamlNode
            {                
                Namespace = prefix != null ? typeContext.GetNamespaceByPrefix(prefix).Name : string.Empty,
                Prefix = prefix ?? string.Empty,
                XamlType = XamlType.Builder.Create(type, typeContext),
                NodeType = isEmpty ? NodeType.EmptyElement : NodeType.Element,
            };
        }

        public ProtoXamlNode NonEmptyElement(Type type, string prefix = null)
        {
            return Element(type, prefix, false);
        }

        public ProtoXamlNode EmptyElement(Type type, string prefix)
        {
            return Element(type, prefix, true);
        }

        internal ProtoXamlNode EmptyElement<T>()
        {
            return EmptyElement<T>(null);
        }

        internal ProtoXamlNode EmptyElement<T>(string prefix)
        {
            return EmptyElement(typeof(T), prefix);
        }

        public ProtoXamlNode AttachableProperty<TParent>(string name, string value, string prefix)
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
                Prefix = prefix,
            };
        }

        // ReSharper disable once UnusedMember.Global
        public ProtoXamlNode EmptyPropertyElement<T>(Expression<Func<T, object>> selector, string prefix)
        {
            return PropertyElement(selector, prefix, isCollapsed: true);
        }

        public ProtoXamlNode NonEmptyPropertyElement<T>(Expression<Func<T, object>> selector, string prefix = null)
        {
            return PropertyElement(selector, prefix, isCollapsed: false);
        }

        public ProtoXamlNode NonEmptyPropertyElement(Type type, string memberName, string prefix)
        {
            return PropertyElement(type, memberName, prefix, isCollapsed: false);
        }

        private ProtoXamlNode PropertyElement(Type type, string memberName, string prefix, bool isCollapsed)
        {
            var property = typeContext.GetXamlType(type).GetMember(memberName);

            return new ProtoXamlNode
            {
                PropertyElement = property,
                Prefix = prefix,
                Namespace = prefix != null ? typeContext.GetNamespaceByPrefix(prefix).Name : string.Empty,
                XamlType = null,
                NodeType =
                    isCollapsed
                        ? NodeType.EmptyPropertyElement
                        : NodeType.PropertyElement
            };
        }

        private ProtoXamlNode PropertyElement<T>(Expression<Func<T, object>> selector, string prefix, bool isCollapsed)
        {
            return PropertyElement(typeof(T), selector.GetFullPropertyName(), prefix, isCollapsed);
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

        public ProtoXamlNode Text()
        {
            return new ProtoXamlNode { Namespace = null, NodeType = NodeType.Text, XamlType = null, };
        }

        public ProtoXamlNode Attribute(XamlMember member, string value, string prefix)
        {
            return new ProtoXamlNode
            {
                PropertyAttribute = member,
                NodeType = NodeType.Attribute,
                PropertyAttributeText = value,
                Prefix = prefix,
            };
        }

        public ProtoXamlNode Attribute<T>(Expression<Func<T, object>> selector, string value, string prefix)
        {
            var xamlMember = typeContext.GetXamlType(typeof (T)).GetMember(selector.GetFullPropertyName());
            return Attribute(xamlMember, value, prefix);
        }
    }
}