namespace OmniXaml.Tests.Parsers.ProtoInstructionParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Classes.Another;
    using Common.NetCore;
    using OmniXaml.Parsers.ProtoParser;
    using Xunit;

    public class PrefixTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {

        private ProtoInstructionParser CreateSut()
        {
            return new ProtoInstructionParser(TypeRuntimeTypeSource);
        }

        [Fact]
        public void SingleCollapsed()
        {
            var sut = CreateSut();
            var actualNodes = sut.Parse("<a:Foreigner xmlns:a=\"another\"/>").ToList();
            var expectedInstructions = new List<ProtoInstruction>
            {
                P.NamespacePrefixDeclaration(AnotherNs),
                P.EmptyElement<Foreigner>(AnotherNs),
            };

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void AttachedProperty()
        {
            var sut = CreateSut();
            var actualNodes = sut.Parse(@"<DummyClass xmlns=""root"" xmlns:a=""another"" a:Foreigner.Property=""Value""></DummyClass>").ToList();

            var ns = "root";

            var expectedInstructions = new Collection<ProtoInstruction>
            {
                P.NamespacePrefixDeclaration("", ns),
                P.NamespacePrefixDeclaration("a", "another"),
                P.NonEmptyElement<DummyClass>(RootNs),
                P.InlineAttachableProperty<Foreigner>("Property", "Value", AnotherNs),
                P.EndTag(),
            };

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void ElementWithPrefixThatIsDefinedAfterwards()
        {
            var sut = CreateSut();
            var actualNodes = sut.Parse(@"<a:DummyChild xmlns:a=""another""></a:DummyChild>").ToList();

            var expectedInstructions = new Collection<ProtoInstruction>
            {
                P.NamespacePrefixDeclaration(AnotherNs),
                P.NonEmptyElement<OmniXaml.Tests.Classes.Another.DummyChild>(AnotherNs),
                P.EndTag(),
            };

            Assert.Equal(expectedInstructions, actualNodes);
        }
    }
}