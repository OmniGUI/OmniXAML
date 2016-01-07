namespace OmniXaml.Tests.Common
{
    using Builder;

    public class GivenARuntimeTypeSourceWithNodeBuilders : GivenARuntimeTypeSource
    {
        protected GivenARuntimeTypeSourceWithNodeBuilders()
        {
            X = new XamlInstructionBuilder(TypeRuntimeTypeSource);
            P = new ProtoInstructionBuilder(TypeRuntimeTypeSource);
        }

        public XamlInstructionBuilder X { get; }

        public ProtoInstructionBuilder P { get; }        
    }
}