namespace OmniXaml.Tests.Parsers
{
    using System;
    using System.Linq.Expressions;
    using Glass;
    using OmniXaml.Parsers.ProtoParser;
    using Typing;

    internal class ProtoNodeBuilder
    {
        private readonly IXamlTypeRepository typeRepository;

        public ProtoNodeBuilder(IXamlTypeRepository typeRepository)
        {
            this.typeRepository = typeRepository;
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

        public ProtoXamlNode NamespacePrefixDeclaration(string ns, string prefix)
        {
            return new ProtoXamlNode
            {
                Namespace = ns,
                XamlType = null,
                NodeType = NodeType.PrefixDefinition,
                Prefix = prefix,
            };
        }

        private ProtoXamlNode Element(Type type, string ns, bool isEmtpy)
        {
            return new ProtoXamlNode
            {
                Namespace = ns,
                XamlType = XamlType.Builder.Create(type, typeRepository),
                NodeType = isEmtpy ? NodeType.EmptyElement : NodeType.Element,
            };
        }

        public ProtoXamlNode NonEmptyElement(Type type, string ns)
        {
            return Element(type, ns, false);
        }

        public ProtoXamlNode EmptyElement(Type type, string ns)
        {
            return Element(type, ns, true);
        }
        internal ProtoXamlNode EmptyElement<T>(string ns)
        {
            return EmptyElement(typeof(T), ns);
        }

        public ProtoXamlNode AttachableProperty<TParent>(string name)
        {
            var type = typeof(TParent);
            var xamlType = typeRepository.Get(type);

            var member = xamlType.GetAttachableMember(name);

            return new ProtoXamlNode
            {
                Namespace = null,
                NodeType = NodeType.Attribute,
                XamlType = null,
                PropertyAttribute = member
            };
        }

        public ProtoXamlNode EmptyPropertyElement<T>(Expression<Func<T, object>> selector, string ns)
        {
            return PropertyElement(selector, ns, isCollapsed: true);
        }

        public ProtoXamlNode NonEmptyPropertyElement<T>(Expression<Func<T, object>> selector, string ns)
        {          
            return PropertyElement(selector, ns, isCollapsed: false);
        }

        private ProtoXamlNode PropertyElement<T>(Expression<Func<T, object>> selector, string ns, bool isCollapsed)
        {
            var property = typeRepository.Get(typeof(T)).GetMember(selector.GetFullPropertyName());

            return new ProtoXamlNode
            {
                PropertyElement = property,
                Namespace = ns,
                XamlType = null,
                NodeType =
                    isCollapsed
                        ? NodeType.EmptyPropertyElement
                        : NodeType.PropertyElement
            };
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

        public ProtoXamlNode Attribute<T>(Expression<Func<T, object>> selector, string value)
        {
            return new ProtoXamlNode
            {
                PropertyAttribute = typeRepository.Get(typeof(T)).GetMember(selector.GetFullPropertyName()),
                NodeType = NodeType.Attribute,
                PropertyAttributeText = value,
            };
        }
    }
}