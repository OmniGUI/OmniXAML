namespace OmniXaml.Tests
{
    using Builder;

    public class GivenAWiringContextWithNodeBuilders : GivenAWiringContext
    {
        private XamlNodeBuilder x;
        private ProtoNodeBuilder p;

        protected GivenAWiringContextWithNodeBuilders()
        {
            x = new XamlNodeBuilder(WiringContext.TypeContext);
            p = new ProtoNodeBuilder(WiringContext.TypeContext, WiringContext.FeatureProvider);
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