﻿namespace OmniXaml.Tests.Parsers.XamlProtoInstructionParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Classes;
    using Classes.Another;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;

    [TestClass]
    public class PrefixTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private IParser<Stream, IEnumerable<ProtoXamlInstruction>> sut;

        [TestInitialize]
        public void Initialize()
        {
            sut = new XamlProtoInstructionParser(WiringContext);
        }

        [TestMethod]
        public void SingleCollapsed()
        {
            var actualNodes = sut.Parse<IEnumerable<ProtoXamlInstruction>>("<x:Foreigner xmlns:x=\"another\"/>").ToList();
            var expectedInstructions = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(AnotherNs),
                P.EmptyElement(typeof (Foreigner), AnotherNs),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualNodes = sut.Parse<IEnumerable<ProtoXamlInstruction>>(@"<DummyClass xmlns=""root"" xmlns:x=""another"" x:Foreigner.Property=""Value""></DummyClass>").ToList();

            var ns = "root";

            var expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration("", ns),
                P.NamespacePrefixDeclaration("x", "another"),
                P.NonEmptyElement(typeof (DummyClass), RootNs),
                P.AttachableProperty<Foreigner>("Property", "Value", AnotherNs),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ElementWithPrefixThatIsDefinedAfterwards()
        {
            var actualNodes = sut.Parse(@"<x:DummyClass xmlns:x=""another""></x:DummyClass>").ToList();

            var expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(AnotherNs),
                P.NonEmptyElement(typeof (DummyClass), AnotherNs),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }
    }
}