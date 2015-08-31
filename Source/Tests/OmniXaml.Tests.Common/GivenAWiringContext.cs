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
        public NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        public NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "a");
        public NamespaceDeclaration SpecialNs { get; } = new NamespaceDeclaration("http://schemas.microsoft.com/winfx/2006/xaml", "x");
    }
}