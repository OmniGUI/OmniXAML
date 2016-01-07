namespace OmniXaml.Tests.Common
{
    using Typing;

    public class GivenAWiringContext
    {
        protected GivenAWiringContext()
        {
            TypeContext = new TestContext();
        }

        protected TestContext TypeContext { get; }
        public NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        public NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "a");
        public NamespaceDeclaration SpecialNs { get; } = new NamespaceDeclaration(CoreTypes.SpecialNamespace, "x");
    }
}