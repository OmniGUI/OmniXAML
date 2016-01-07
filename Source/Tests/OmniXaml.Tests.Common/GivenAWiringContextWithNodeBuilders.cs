namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;

    public class GivenAWiringContextWithNodeBuilders : GivenAWiringContext
    {
        protected GivenAWiringContextWithNodeBuilders(IEnumerable<Assembly> assemblies) : base()
        {
            X = new XamlInstructionBuilder(TypeContext);
            P = new ProtoInstructionBuilder(TypeContext);
        }

        public XamlInstructionBuilder X { get; }

        public ProtoInstructionBuilder P { get; }        
    }
}