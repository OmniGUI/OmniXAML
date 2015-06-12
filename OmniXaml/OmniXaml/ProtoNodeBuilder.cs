namespace OmniXaml
{
    using System;
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
            return new ProtoXamlNode
            {
                Namespace = ns,
                XamlType = null,
                NodeType = NodeType.PrefixDefinition,
                Prefix = prefix,
            };
        }

        private ProtoXamlNode Element(Type type, string prefix, bool isEmtpy)
        {
            return new ProtoXamlNode
            {
                // TODO:
                Namespace = typeContext.GetNamespaceByPrefix(prefix).Name,
                Prefix = prefix,
                XamlType = XamlType.Builder.Create(type, typeContext),
                NodeType = isEmtpy ? NodeType.EmptyElement : NodeType.Element,
            };
        }

        public ProtoXamlNode NonEmptyElement(Type type, string prefix)
        {
            return Element(type, prefix, false);
        }

        public ProtoXamlNode EmptyElement(Type type, string prefix)
        {
            return Element(type, prefix, true);
        }
        internal ProtoXamlNode EmptyElement<T>(string ns)
        {
            return EmptyElement(typeof(T), "");
        }

        public ProtoXamlNode AttachableProperty<TParent>(string name, string value, string prefix)
        {
            var type = typeof(TParent);
            var xamlType = typeContext.Get(type);

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

        public ProtoXamlNode NonEmptyPropertyElement<T>(Expression<Func<T, object>> selector, string prefix)
        {
            return PropertyElement(selector, prefix, isCollapsed: false);
        }

        public ProtoXamlNode NonEmptyPropertyElement(Type type, string memberName)
        {
            return PropertyElement(type, memberName, isCollapsed: false);
        }

        private ProtoXamlNode PropertyElement(Type type, string memberName, bool isCollapsed)
        {
            var property = typeContext.Get(type).GetMember(memberName);

            return new ProtoXamlNode
            {
                PropertyElement = property,
                Prefix = string.Empty,
                Namespace = null, // TODO
                XamlType = null,
                NodeType =
                    isCollapsed
                        ? NodeType.EmptyPropertyElement
                        : NodeType.PropertyElement
            };
        }

        private ProtoXamlNode PropertyElement<T>(Expression<Func<T, object>> selector, string prefix, bool isCollapsed)
        {
            return PropertyElement(typeof(T), selector.GetFullPropertyName(), isCollapsed);
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

        public ProtoXamlNode Attribute(XamlMember member, string value)
        {
            return new ProtoXamlNode
            {
                PropertyAttribute = member,
                NodeType = NodeType.Attribute,
                PropertyAttributeText = value,
                Prefix = string.Empty,
            };
        }

        public ProtoXamlNode Attribute<T>(Expression<Func<T, object>> selector, string value)
        {
            return Attribute(typeContext.Get(typeof (T)).GetMember(selector.GetFullPropertyName()), value);           
        }
    }
}