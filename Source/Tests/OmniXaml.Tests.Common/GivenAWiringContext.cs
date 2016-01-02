namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using System.Reflection;
    using Typing;

    public class GivenAWiringContext
    {
        protected GivenAWiringContext(IEnumerable<Assembly> assemblies)
        {
            WiringContext = new WiringContextMock(new TypeFactory(), assemblies);
            TypeContext = WiringContext.TypeContext;
        }

        protected ITypeContext TypeContext { get; set; }

        protected WiringContextMock WiringContext { get; private set; }

        public NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        public NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "a");
        public NamespaceDeclaration SpecialNs { get; } = new NamespaceDeclaration(CoreTypes.SpecialNamespace, "x");
    }
}