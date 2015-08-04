namespace OmniXaml.Tests
{
    using Classes;

    public class GivenAWiringContext
    {
        protected WiringContext WiringContext => DummyWiringContext.Create(new TypeFactory());
        protected NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        protected NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "x");
    }
}