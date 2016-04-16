namespace OmniXaml
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using Glass.Core;
    using Parsers.ProtoParser;
    using Typing;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ProtoInstructionBuilder
    {
        private readonly IRuntimeTypeSource typeSource;

        public ProtoInstructionBuilder(IRuntimeTypeSource typeSource)
        {
            this.typeSource = typeSource;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public PrefixRegistrationMode PrefixRegistrationMode { get; } = PrefixRegistrationMode.Automatic;

        public ProtoInstruction None()
        {
            return new ProtoInstruction
            {
                Namespace = null,
                XamlType = null,
                NodeType = NodeType.None
            };
        }

        public ProtoInstruction NamespacePrefixDeclaration(NamespaceDeclaration ns)
        {
            return NamespacePrefixDeclaration(ns.Prefix, ns.Namespace);
        }

        public ProtoInstruction NamespacePrefixDeclaration(string prefix, string ns)
        {
            if (PrefixRegistrationMode == PrefixRegistrationMode.Automatic)
            {
                var prefixRegistration = new PrefixRegistration(prefix, ns);
                if (!typeSource.RegisteredPrefixes.Contains(prefixRegistration))
                {
                    typeSource.RegisterPrefix(prefixRegistration);
                }
            }

            return new ProtoInstruction
            {
                Namespace = ns,
                XamlType = null,
                NodeType = NodeType.PrefixDefinition,
                Prefix = prefix,
            };
        }

        private ProtoInstruction Element(Type type, NamespaceDeclaration nsDecl, bool isEmpty)
        {
            return new ProtoInstruction
            {
                Namespace = nsDecl.Namespace,
                Prefix = nsDecl.Prefix,
                XamlType = typeSource.GetByType(type),
                NodeType = isEmpty ? NodeType.EmptyElement : NodeType.Element,
            };
        }

        public ProtoInstruction NonEmptyElement<T>(NamespaceDeclaration nsDecl = null)
        {
            return Element(typeof(T), nsDecl, false);
        }

        public ProtoInstruction NonEmptyElement(Type type, NamespaceDeclaration nsDecl = null)
        {
            return Element(type, nsDecl, false);
        }

        public ProtoInstruction EmptyElement(Type type, NamespaceDeclaration nsDecl)
        {
            return Element(type, nsDecl, true);
        }

        internal ProtoInstruction EmptyElement<T>()
        {
            return EmptyElement<T>(null);
        }

        public ProtoInstruction EmptyElement<T>(NamespaceDeclaration namespaceDeclaration)
        {
            return EmptyElement(typeof(T), namespaceDeclaration);
        }

        public ProtoInstruction InlineAttachableProperty<TParent>(string name, string value, NamespaceDeclaration namespaceDeclaration)
        {
            var type = typeof(TParent);
            var xamlType = typeSource.GetByType(type);

            var member = xamlType.GetAttachableMember(name);

            return new ProtoInstruction
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
        public ProtoInstruction EmptyPropertyElement<T>(Expression<Func<T, object>> selector, NamespaceDeclaration namespaceDeclaration)
        {
            return PropertyElement(selector, namespaceDeclaration, isCollapsed: true);
        }

        public ProtoInstruction NonEmptyPropertyElement<T>(Expression<Func<T, object>> selector, NamespaceDeclaration namespaceDeclaration = null)
        {
            return PropertyElement(selector, namespaceDeclaration, isCollapsed: false);
        }

        public ProtoInstruction NonEmptyPropertyElement(Type type, string memberName, NamespaceDeclaration namespaceDeclaration)
        {
            return PropertyElement(type, memberName, namespaceDeclaration, isCollapsed: false);
        }

        private ProtoInstruction PropertyElement(Type type, string memberName, NamespaceDeclaration namespaceDeclaration, bool isCollapsed)
        {
            var property = typeSource.GetByType(type).GetMember(memberName);

            return new ProtoInstruction
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

        private ProtoInstruction PropertyElement<T>(Expression<Func<T, object>> selector, NamespaceDeclaration namespaceDeclaration, bool isCollapsed)
        {
            return PropertyElement(typeof(T), selector.GetFullPropertyName(), namespaceDeclaration, isCollapsed);
        }

        public ProtoInstruction EndTag()
        {
            return new ProtoInstruction
            {
                Namespace = null,
                XamlType = null,
                NodeType = NodeType.EndTag
            };
        }

        public ProtoInstruction Text(string text = null)
        {
            return new ProtoInstruction { Namespace = null, NodeType = NodeType.Text, XamlType = null, Text = text };
        }

        public ProtoInstruction Attribute(MemberBase member, string value, string prefix)
        {
            return new ProtoInstruction
            {
                PropertyAttribute = member,
                NodeType = NodeType.Attribute,
                PropertyAttributeText = value,
                Prefix = prefix,
            };
        }

        public ProtoInstruction Attribute<T>(Expression<Func<T, object>> selector, string value, string prefix)
        {
            var xamlMember = typeSource.GetByType(typeof(T)).GetMember(selector.GetFullPropertyName());
            return Attribute(xamlMember, value, prefix);
        }

        public ProtoInstruction Key(string value)
        {
            return Attribute(CoreTypes.sKey, value, "x");
        }

        public ProtoInstruction Directive(Directive directive, string value)
        {
            return Attribute(directive, value, "x");
        }

        public ProtoInstruction ExpandedAttachedProperty<TParent>(string name, NamespaceDeclaration namespaceDeclaration)
        {
            return ExpandedAttachedProperty(typeof (TParent), name, namespaceDeclaration);
        }

        public ProtoInstruction ExpandedAttachedProperty(Type owner, string name, NamespaceDeclaration namespaceDeclaration)
        {
            var xamlType = typeSource.GetByType(owner);

            var member = xamlType.GetAttachableMember(name);

            return new ProtoInstruction
            {
                Namespace = namespaceDeclaration.Namespace,
                NodeType = NodeType.PropertyElement,
                XamlType = null,
                PropertyElement = member,
                Prefix = namespaceDeclaration.Prefix,
            };
        }
    }
}