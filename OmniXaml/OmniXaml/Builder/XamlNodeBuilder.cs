namespace OmniXaml.Builder
{
    using System;
    using System.Linq.Expressions;
    using Glass;
    using Typing;

    public class XamlNodeBuilder
    {
        private readonly IXamlTypeRepository registry;

        public XamlNodeBuilder(IXamlTypeRepository registry)
        {
            this.registry = registry;
        }

        public XamlNode None()
        {
            return new XamlNode(XamlNodeType.None);
        }

        public XamlNode NamespacePrefixDeclaration(NamespaceDeclaration ns)
        {
            return NamespacePrefixDeclaration(ns.Namespace, ns.Prefix);
        }

        public XamlNode NamespacePrefixDeclaration(string ns, string prefix)
        {
            return new XamlNode(XamlNodeType.NamespaceDeclaration, new NamespaceDeclaration(ns, prefix));
        }

        public XamlNode StartObject(Type type)
        {
            return new XamlNode(XamlNodeType.StartObject, registry.GetXamlType(type));
        }

        public XamlNode StartObject<T>()
        {
            return StartObject(typeof(T));
        }

        public XamlNode EndObject()
        {
            return new XamlNode(XamlNodeType.EndObject, null);
        }

        public XamlNode StartMember<T>(Expression<Func<T, object>> selector)
        {
            var name = selector.GetFullPropertyName();
            var xamlMember = registry.GetXamlType(typeof(T)).GetMember(name);
            return new XamlNode(XamlNodeType.StartMember, xamlMember);
        }

        public XamlNode EndMember()
        {
            return new XamlNode(XamlNodeType.EndMember);
        }

        public XamlNode Value(object value)
        {
            return new XamlNode(XamlNodeType.Value, value);
        }

        public XamlNode Items()
        {
            return new XamlNode(XamlNodeType.StartMember, CoreTypes.Items);
        }

        public XamlNode GetObject()
        {
            return new XamlNode(XamlNodeType.GetObject);
        }

        public XamlNode MarkupExtensionArguments()
        {
            return new XamlNode(XamlNodeType.StartMember, CoreTypes.MarkupExtensionArguments);
        }

        public XamlNode StartDirective(string directive)
        {            
            return new XamlNode(XamlNodeType.StartMember, new XamlDirective(directive));
        }
    }
}