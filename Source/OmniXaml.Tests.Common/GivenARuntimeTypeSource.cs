namespace OmniXaml.Tests.Common
{
    using Builder;
    using Typing;

    public class GivenARuntimeTypeSource
    {
        protected GivenARuntimeTypeSource()
        {
            RuntimeTypeSource = new TestRuntimeTypeSource();
            X = new XamlInstructionBuilder(RuntimeTypeSource);
            P = new ProtoInstructionBuilder(RuntimeTypeSource);
        }

        protected TestRuntimeTypeSource RuntimeTypeSource { get; }
        public NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        public NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "a");
        public NamespaceDeclaration SpecialNs { get; } = new NamespaceDeclaration(CoreTypes.SpecialNamespace, "x");
        public XamlInstructionBuilder X { get; }
        public ProtoInstructionBuilder P { get; }
    }
}