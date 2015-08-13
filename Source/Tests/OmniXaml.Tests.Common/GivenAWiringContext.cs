namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using System.Reflection;

    public class GivenAWiringContext
    {
        private readonly IEnumerable<Assembly> assemblies;

        protected GivenAWiringContext(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        protected IWiringContext WiringContext => new DummyWiringContext(new TypeFactory(), assemblies);
        protected NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        protected NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "x");
    }
}