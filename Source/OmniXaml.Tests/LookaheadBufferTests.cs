namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Common;
    using Xunit;
    using Resources;

    public class LookaheadBufferTests : GivenARuntimeTypeSource
    {
        private readonly InstructionResources resources;

        public LookaheadBufferTests()
        {
            resources = new InstructionResources(this);
        }

        [Fact]
        public void LookAheadTest()
        {
            var look = new List<Instruction>
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
            Assert.Equal(8, count);
        }

        [Fact]
        public void LookAheadTestStartZero()
        {
            var instructions = new List<Instruction>();

            var enumerator = instructions.GetEnumerator();
            enumerator.MoveNext();
            var count = LookaheadBuffer.GetUntilEndOfRoot(enumerator).Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public void LookAheadTestStartMiniumLength()
        {
            var look = new List<Instruction>
            {
                X.StartObject<Style>(),
                X.EndObject()
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookaheadBuffer.GetUntilEndOfRoot(enumerator).Count();
            Assert.Equal(2, count);
        }

        [Fact]
        public void LookAheadTest10()
        {
            var look = new List<Instruction>
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
            Assert.Equal(10, count);
        }

        [Fact]
        public void LookAheadTestWithGetObjectAndCollection()
        {
            var look = resources.ComboBoxUnsorted;
            var expectedCount = look.Count;

            for (var t = 0; t < 4; t++)
            {
                look.Add(new Instruction(InstructionType.Value, "Noise"));
            }

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookaheadBuffer.GetUntilEndOfRoot(enumerator).Count();

            Assert.Equal(expectedCount, count);
        }
    }
}