namespace OmniXaml.Builder
{
    using System;
    using System.Linq.Expressions;
    using Glass;
    using Typing;

    public class XamlInstructionBuilder
    {
        private readonly IXamlTypeRepository registry;

        public XamlInstructionBuilder(IXamlTypeRepository registry)
        {
            this.registry = registry;
        }

        public XamlInstruction None()
        {
            return new XamlInstruction(XamlInstructionType.None);
        }

        public XamlInstruction NamespacePrefixDeclaration(NamespaceDeclaration ns)
        {
            return NamespacePrefixDeclaration(ns.Namespace, ns.Prefix);
        }

        public XamlInstruction NamespacePrefixDeclaration(string ns, string prefix)
        {
            return new XamlInstruction(XamlInstructionType.NamespaceDeclaration, new NamespaceDeclaration(ns, prefix));
        }

        public XamlInstruction StartObject(Type type)
        {
            return new XamlInstruction(XamlInstructionType.StartObject, registry.GetXamlType(type));
        }

        public XamlInstruction StartObject<T>()
        {
            return StartObject(typeof(T));
        }

        public XamlInstruction EndObject()
        {
            return new XamlInstruction(XamlInstructionType.EndObject, null);
        }

        public XamlInstruction StartMember<T>(Expression<Func<T, object>> selector)
        {
            var name = selector.GetFullPropertyName();
            var xamlMember = registry.GetXamlType(typeof(T)).GetMember(name);
            return new XamlInstruction(XamlInstructionType.StartMember, xamlMember);
        }

        public XamlInstruction EndMember()
        {
            return new XamlInstruction(XamlInstructionType.EndMember);
        }

        public XamlInstruction Value(object value)
        {
            return new XamlInstruction(XamlInstructionType.Value, value);
        }

        public XamlInstruction Items()
        {
            return new XamlInstruction(XamlInstructionType.StartMember, CoreTypes.Items);
        }

        public XamlInstruction GetObject()
        {
            return new XamlInstruction(XamlInstructionType.GetObject);
        }

        public XamlInstruction MarkupExtensionArguments()
        {
            return new XamlInstruction(XamlInstructionType.StartMember, CoreTypes.MarkupExtensionArguments);
        }

        public XamlInstruction StartDirective(string directive)
        {            
            return new XamlInstruction(XamlInstructionType.StartMember, new XamlDirective(directive));
        }
    }
}