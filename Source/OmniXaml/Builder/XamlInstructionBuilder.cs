namespace OmniXaml.Builder
{
    using System;
    using System.Linq.Expressions;
    using Glass.Core;
    using Typing;

    public class XamlInstructionBuilder
    {
        private readonly ITypeRepository registry;

        public XamlInstructionBuilder(ITypeRepository registry)
        {
            this.registry = registry;
        }

        public Instruction None()
        {
            return new Instruction(InstructionType.None);
        }

        public Instruction NamespacePrefixDeclaration(NamespaceDeclaration ns)
        {
            return NamespacePrefixDeclaration(ns.Namespace, ns.Prefix);
        }

        public Instruction NamespacePrefixDeclaration(string ns, string prefix)
        {
            return new Instruction(InstructionType.NamespaceDeclaration, new NamespaceDeclaration(ns, prefix));
        }

        public Instruction StartObject(Type type)
        {
            return new Instruction(InstructionType.StartObject, registry.GetByType(type));
        }

        public Instruction StartObject<T>()
        {
            return StartObject(typeof(T));
        }

        public Instruction EndObject()
        {
            return new Instruction(InstructionType.EndObject, null);
        }

        public Instruction StartMember<T>(Expression<Func<T, object>> selector)
        {
            var name = selector.GetFullPropertyName();
            var xamlMember = registry.GetByType(typeof(T)).GetMember(name);
            return new Instruction(InstructionType.StartMember, xamlMember);
        }

        public Instruction EndMember()
        {
            return new Instruction(InstructionType.EndMember);
        }

        public Instruction Value(string value)
        {
            return new Instruction(InstructionType.Value, value);
        }

        public Instruction Items()
        {
            return new Instruction(InstructionType.StartMember, CoreTypes.Items);
        }

        public Instruction GetObject()
        {
            return new Instruction(InstructionType.GetObject);
        }

        public Instruction MarkupExtensionArguments()
        {
            return new Instruction(InstructionType.StartMember, CoreTypes.MarkupExtensionArguments);
        }

        public Instruction Name()
        {
            return StartDirective("Name");
        }

        public Instruction Key()
        {
            return StartDirective("Key");
        }

        public Instruction Initialization()
        {
            return StartDirective("_Initialization");
        }

        private static Instruction StartDirective(string directive)
        {            
            return new Instruction(InstructionType.StartMember, new Directive(directive));
        }

        public Instruction UnknownContent()
        {
            return new Instruction(InstructionType.StartMember, CoreTypes.UnknownContent);
        }

        public Instruction AttachableProperty<TParent>(string name)
        {
            var type = typeof(TParent);
            var xamlType = registry.GetByType(type);
            var member = xamlType.GetAttachableMember(name);

            return new Instruction(InstructionType.StartMember, member);
        }
    }
}