namespace OmniXaml.Tests.Parsers.XamlProtoInstructionParserTests
{
    using OmniXaml.Parsers.ProtoParser;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Classes;
    using Classes.Another;
    using Common.NetCore;
    using System.Linq;
    using Xunit;


    public class PrefixTests : GivenAWiringContextWithNodeBuildersNetCore
    {

        private XamlProtoInstructionParser CreateSut()
        {
            return new XamlProtoInstructionParser(WiringContext.TypeContext);
        }

        [Fact]
        public void SingleCollapsed()
        {
            var sut = CreateSut();
            var actualNodes = sut.Parse("<a:Foreigner xmlns:a=\"another\"/>").ToList();
            var expectedInstructions = new List<ProtoXamlInstruction>
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

            var expectedInstructions = new Collection<ProtoXamlInstruction>
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
            var actualNodes = sut.Parse(@"<a:DummyClass xmlns:a=""another""></a:DummyClass>").ToList();

            var expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(AnotherNs),
                P.NonEmptyElement<DummyClass>(AnotherNs),
                P.EndTag(),
            };

            Assert.Equal(expectedInstructions, actualNodes);
        }
    }
}