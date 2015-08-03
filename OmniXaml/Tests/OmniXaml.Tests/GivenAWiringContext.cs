namespace OmniXaml.Tests
{
    using Classes;

    public class GivenAWiringContext
    {
        protected WiringContext WiringContext => DummyWiringContext.Create(new TypeFactory());
        protected readonly NamespaceDeclaration rootNs = new NamespaceDeclaration("root", string.Empty);
        protected readonly NamespaceDeclaration anotherNs = new NamespaceDeclaration("another", "x");
    }
}