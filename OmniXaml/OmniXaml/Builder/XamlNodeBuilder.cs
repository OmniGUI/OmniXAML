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

        public XamlInstruction None()
        {
            return new XamlInstruction(XamlNodeType.None);
        }

        public XamlInstruction NamespacePrefixDeclaration(NamespaceDeclaration ns)
        {
            return NamespacePrefixDeclaration(ns.Namespace, ns.Prefix);
        }

        public XamlInstruction NamespacePrefixDeclaration(string ns, string prefix)
        {
            return new XamlInstruction(XamlNodeType.NamespaceDeclaration, new NamespaceDeclaration(ns, prefix));
        }

        public XamlInstruction StartObject(Type type)
        {
            return new XamlInstruction(XamlNodeType.StartObject, registry.GetXamlType(type));
        }

        public XamlInstruction StartObject<T>()
        {
            return StartObject(typeof(T));
        }

        public XamlInstruction EndObject()
        {
            return new XamlInstruction(XamlNodeType.EndObject, null);
        }

        public XamlInstruction StartMember<T>(Expression<Func<T, object>> selector)
        {
            var name = selector.GetFullPropertyName();
            var xamlMember = registry.GetXamlType(typeof(T)).GetMember(name);
            return new XamlInstruction(XamlNodeType.StartMember, xamlMember);
        }

        public XamlInstruction EndMember()
        {
            return new XamlInstruction(XamlNodeType.EndMember);
        }

        public XamlInstruction Value(object value)
        {
            return new XamlInstruction(XamlNodeType.Value, value);
        }

        public XamlInstruction Items()
        {
            return new XamlInstruction(XamlNodeType.StartMember, CoreTypes.Items);
        }

        public XamlInstruction GetObject()
        {
            return new XamlInstruction(XamlNodeType.GetObject);
        }

        public XamlInstruction MarkupExtensionArguments()
        {
            return new XamlInstruction(XamlNodeType.StartMember, CoreTypes.MarkupExtensionArguments);
        }

        public XamlInstruction StartDirective(string directive)
        {            
            return new XamlInstruction(XamlNodeType.StartMember, new XamlDirective(directive));
        }
    }
}