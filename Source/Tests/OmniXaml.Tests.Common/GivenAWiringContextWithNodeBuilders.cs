namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;

    public class GivenAWiringContextWithNodeBuilders : GivenAWiringContext
    {
        protected GivenAWiringContextWithNodeBuilders(IEnumerable<Assembly> assemblies) : base(assemblies)
        {
            X = new XamlInstructionBuilder(WiringContext.TypeContext);
            P = new ProtoInstructionBuilder(WiringContext.TypeContext);
        }

        public XamlInstructionBuilder X { get; }

        public ProtoInstructionBuilder P { get; }        
    }
}