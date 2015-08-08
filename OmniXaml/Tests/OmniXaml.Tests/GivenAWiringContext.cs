namespace OmniXaml.Tests
{
    using Builder;
    using Classes;

    public class GivenAWiringContext
    {
        protected WiringContext WiringContext => DummyWiringContext.Create(new TypeFactory());
        protected NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        protected NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "x");
    }

    public class GivenAWiringContextWithNodeBuilders : GivenAWiringContext
    {
        private XamlNodeBuilder x;
        private ProtoNodeBuilder p;

        protected GivenAWiringContextWithNodeBuilders()
        {
            x = new XamlNodeBuilder(this.WiringContext.TypeContext);
        }

        protected XamlNodeBuilder X
        {
            get { return x; }
            set { x = value; }
        }

        public ProtoNodeBuilder P
        {
            get { return p; }
            set { p = value; }
        }
    }
}