namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using Builder;
    using ObjectAssembler;
    using TypeConversion;
    using Typing;

    public class GivenARuntimeTypeSource
    {
        protected GivenARuntimeTypeSource()
        {
            RuntimeTypeSource = new TestRuntimeTypeSource();
            X = new XamlInstructionBuilder(RuntimeTypeSource);
            P = new ProtoInstructionBuilder(RuntimeTypeSource);
        }

        protected TestRuntimeTypeSource RuntimeTypeSource { get; }
        public NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        public NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "a");
        public NamespaceDeclaration SpecialNs { get; } = new NamespaceDeclaration(CoreTypes.SpecialNamespace, "x");
        public XamlInstructionBuilder X { get; }
        public ProtoInstructionBuilder P { get; }

        protected IObjectAssembler CreateObjectAssembler()
        {
            var topDownValueContext = new TopDownValueContext();
            var objectAssembler = new ObjectAssembler(RuntimeTypeSource, new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>()));
            return objectAssembler;
        }
    }
}