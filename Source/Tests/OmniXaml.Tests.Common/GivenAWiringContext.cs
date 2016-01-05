namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using System.Reflection;
    using Typing;

    public class GivenAWiringContext
    {
        protected GivenAWiringContext(IEnumerable<Assembly> assemblies)
        {
            TypeContext = OmniXaml.TypeContext.FromAttributes(assemblies);
            TypeContext.RegisterPrefix(new PrefixRegistration("", "root"));
            TypeContext.RegisterPrefix(new PrefixRegistration("x", "another"));
        }

        protected ITypeContext TypeContext { get; set; }
        public NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        public NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "a");
        public NamespaceDeclaration SpecialNs { get; } = new NamespaceDeclaration(CoreTypes.SpecialNamespace, "x");
    }
}