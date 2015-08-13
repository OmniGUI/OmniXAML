namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LookaheadBufferTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        [TestMethod]
        public void LookAheadTest()
        {
            var look = new List<XamlInstruction>
            {
                X.StartObject<Style>(),
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
                X.EndObject(),
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookaheadBuffer.GetUntilEndOfRoot(enumerator).Count();
            Assert.AreEqual(8, count);
        }

        [TestMethod]
        public void LookAheadTestStartZero()
        {
            var instructions = new List<XamlInstruction>();

            var enumerator = instructions.GetEnumerator();
            enumerator.MoveNext();
            var count = LookaheadBuffer.GetUntilEndOfRoot(enumerator).Count();
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void LookAheadTestStartMiniumLength()
        {
            var look = new List<XamlInstruction>
            {
                X.StartObject<Style>(),
                X.EndObject()
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookaheadBuffer.GetUntilEndOfRoot(enumerator).Count();
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void LookAheadTest10()
        {
            var look = new List<XamlInstruction>
            {
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookaheadBuffer.GetUntilEndOfRoot(enumerator).Count();
            Assert.AreEqual(10, count);
        }
    }
}