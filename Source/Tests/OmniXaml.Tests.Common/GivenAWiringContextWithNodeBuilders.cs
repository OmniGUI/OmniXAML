namespace OmniXaml.Tests.Common
{
    using Builder;

    public class GivenARuntimeTypeContextWithNodeBuilders : GivenARuntimeTypeContext
    {
        protected GivenARuntimeTypeContextWithNodeBuilders()
        {
            X = new XamlInstructionBuilder(TypeRuntimeTypeSource);
            P = new ProtoInstructionBuilder(TypeRuntimeTypeSource);
        }

        public XamlInstructionBuilder X { get; }

        public ProtoInstructionBuilder P { get; }        
    }
}