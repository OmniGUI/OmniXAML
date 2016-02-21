namespace OmniXaml.Tests.Common
{
    using Builder;

    public class GivenARuntimeTypeSourceWithNodeBuilders : GivenARuntimeTypeSource
    {
        protected GivenARuntimeTypeSourceWithNodeBuilders()
        {
            X = new XamlInstructionBuilder(RuntimeTypeSource);
            P = new ProtoInstructionBuilder(RuntimeTypeSource);
        }

        public XamlInstructionBuilder X { get; }

        public ProtoInstructionBuilder P { get; }        
    }
}