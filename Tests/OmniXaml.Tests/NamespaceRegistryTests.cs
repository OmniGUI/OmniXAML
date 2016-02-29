namespace OmniXaml.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Builder;
    using Classes;
    using Xunit;
    using Typing;

    public class NamespaceRegistryTests
    {
        private NamespaceRegistry registry;
        private Type type;
        private string clrNamespace;

        public NamespaceRegistryTests()
        {
            type = typeof(DummyClass);
            registry = new NamespaceRegistry();
            registry.RegisterPrefix(new PrefixRegistration("my", "target"));
            clrNamespace = $"clr-namespace:{type.Namespace};Assembly={type.GetTypeInfo().Assembly}";
            registry.RegisterPrefix(new PrefixRegistration("clr", clrNamespace));
            registry.AddNamespace(
                XamlNamespace.Map("target")
                    .With(new[] { Route.Assembly(type.Assembly).WithNamespaces(new[] { type.Namespace }) }));
        }

        [Fact]
        public void RegisterPrefixTest()
        {
            Assert.Equal(
                registry.RegisteredPrefixes.ToList(),
                new Collection<PrefixRegistration> {new PrefixRegistration("my", "target"), new PrefixRegistration("clr", clrNamespace)});
        }

        [Fact]
        public void GetClrNsByPrefix()
        {
            var clrNs = registry.GetClrNamespaceByPrefix("clr");
            var expected = new ClrNamespace(type.GetTypeInfo().Assembly, type.Namespace);
            Assert.Equal(expected, clrNs);            
        }

        [Fact]
        public void GetXamlNamespaceOfNotRegisteredPrefix()
        {
            Assert.Null(registry.GetXamlNamespace("unknown_namespace"));
        }

        [Fact]
        public void GetNamespaceByPrefix()
        {
            var ns = registry.GetXamlNamespaceByPrefix("my");
            Assert.Equal("target", ns.Name);
        }

        [Fact]
        public void GetNamespace()
        {            
            var ns = registry.GetXamlNamespace("target");
            Assert.Equal("target", ns.Name);
        }
    }
}
