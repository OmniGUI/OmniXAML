namespace OmniXaml.Tests.Parsers
{
    using System;
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
                NodeType = ProtoNodeType.None
            };
        }

        public ProtoXamlNode NamespacePrefixDeclaration(string ns, string prefix)
        {
            return new ProtoXamlNode
            {
                Namespace = ns,                                
                XamlType = null,
                NodeType = ProtoNodeType.PrefixDefinition,
                Prefix = prefix,
            };
        }

        private ProtoXamlNode WithElement(Type type, string ns, bool isEmtpy)
        {
            return new ProtoXamlNode
            {
                Namespace = ns,
                XamlType = XamlType.Builder.Create(type, typeRepository),
                NodeType = isEmtpy ? ProtoNodeType.EmptyElement : ProtoNodeType.Element,
            };
        }

        public ProtoXamlNode ElementNonCollapsed(Type type, string ns)
        {
            return WithElement(type, ns, false);
        }

        public ProtoXamlNode ElementCollapsed(Type type, string ns)
        {
            return WithElement(type, ns, true);
        }

        public ProtoXamlNode PropertyCollapsed(string ns)
        {
            return Property(ns, true);
        }

        public ProtoXamlNode AttachableProperty<TParent>(string name)
        {
            var type = typeof(TParent);
            var xamlType = typeRepository.Get(type);

            var member = xamlType.GetAttachableMember(name);

            return new ProtoXamlNode
            {
                Namespace = null,
                NodeType = ProtoNodeType.Attribute,
                XamlType = null,
                PropertyAttribute = member
            };
        }

        public ProtoXamlNode PropertyNonCollapsed(string ns)
        {
            return Property(ns, false);
        }

        private ProtoXamlNode Property(string ns, bool isCollapsed)
        {
            return new ProtoXamlNode
                       {
                           Namespace = ns,
                           XamlType = null,
                           NodeType =
                               isCollapsed
                                   ? ProtoNodeType.EmptyPropertyElement
                                   : ProtoNodeType.PropertyElement
                       };
        }

        public ProtoXamlNode EndTag()
        {
            return new ProtoXamlNode
            {
                Namespace = null,
                XamlType = null,
                NodeType = ProtoNodeType.EndTag
            };
        }

        public ProtoXamlNode Text()
        {
            return new ProtoXamlNode { Namespace = null, NodeType = ProtoNodeType.Text, XamlType = null, };
        }
    }
}