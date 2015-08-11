namespace OmniXaml.Tests
{
    using Builder;

    public class GivenAWiringContextWithNodeBuilders : GivenAWiringContext
    {
        private XamlInstructionBuilder x;
        private ProtoInstructionBuilder p;

        protected GivenAWiringContextWithNodeBuilders()
        {
            x = new XamlInstructionBuilder(WiringContext.TypeContext);
            p = new ProtoInstructionBuilder(WiringContext.TypeContext, WiringContext.FeatureProvider);
        }

        protected XamlInstructionBuilder X
        {
            get { return x; }
            set { x = value; }
        }

        public ProtoInstructionBuilder P
        {
            get { return p; }
            set { p = value; }
        }
    }
}