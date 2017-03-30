namespace OmniXaml.Tests.Rework2
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Model;
    using OmniXaml.Rework;
    using ReworkPhases;
    using Xunit;

    public class MemberAssigmentApplierTests
    {
        [Fact]
        public void AssignTestNoConversion()
        {
            var sut = CreateSut(new FuncStringConverter(s => (true, s)));
            var inflatedMemberAssignment = new InflatedMemberAssignment()
            {
                Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                Children = new List<InflatedNode>()
                {
                    new InflatedNode()
                    {
                        Instance = "Pepito",                            
                    },
                }
            };
            var textBlock = new TextBlock();
            var success = sut.TryApply(inflatedMemberAssignment, textBlock);

            Assert.True(success);
            Assert.Equal("Pepito", textBlock.Text);
        }

        [Fact]
        public void ConvertFail()
        {
            var sut = CreateSut(new FuncStringConverter(s => (false, null)));
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<Window>(w => w.Height),
                Children = new List<InflatedNode>()
                {
                    new InflatedNode()
                    {
                        Instance = "12.5",
                    },
                }
            };
            var window = new Window();
            var success = sut.TryApply(inflatedMemberAssignment, window);

            Assert.False(success);            
        }


        [Fact]
        public void ConvertSuccess()
        {
            var sut = CreateSut(new FuncStringConverter(s => (true, double.Parse(s, CultureInfo.InvariantCulture))));
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<Window>(w => w.Height),
                Children = new List<InflatedNode>()
                {
                    new InflatedNode()
                    {
                        Instance = "12.5",
                    },
                }
            };
            var window = new Window();
            var success = sut.TryApply(inflatedMemberAssignment, window);

            Assert.True(success);
            Assert.Equal(12.5, window.Height);
        }

        private static MemberAssigmentApplier CreateSut(IStringSourceValueConverter funcStringConverter)
        {
            return new MemberAssigmentApplier(funcStringConverter);
        }
    }
}