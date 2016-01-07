namespace OmniXaml.Tests.Common
{
    using Typing;

    public class GivenARuntimeTypeContext
    {
        protected GivenARuntimeTypeContext()
        {
            TypeRuntimeTypeSource = new TestRuntimeTypeSource();
        }

        protected TestRuntimeTypeSource TypeRuntimeTypeSource { get; }
        public NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        public NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "a");
        public NamespaceDeclaration SpecialNs { get; } = new NamespaceDeclaration(CoreTypes.SpecialNamespace, "x");
    }
}