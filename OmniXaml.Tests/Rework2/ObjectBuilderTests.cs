namespace OmniXaml.Tests.Rework2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using ReworkPhases;
    using Xunit;

    public class ObjectBuilderTests
    {
        [Fact]
        public void SourceValue()
        {
            var converter = new FuncStringConverterExtended((s, t) => (true, Convert.ChangeType(s, t)));
            var sut = new ObjectBuilder(null, converter, null);
            var result = sut.Build(new ConstructionNode(typeof(int)) {SourceValue = "1"});
            Assert.Equal(1, result.Instance);
        }

        [Fact]
        public void NonConvertibleInstance()
        {
            var textBlock = new TextBlock();
            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(textBlock));
            var sut = new ObjectBuilder(creator, null, null);
            
            var result = sut.Build(new ConstructionNode(typeof(TextBlock)));

            Assert.Equal(textBlock, result.Instance);
        }

        [Fact]
        public void Children()
        {
            var converter = new FuncStringConverterExtended((s, t) => (true, Convert.ChangeType(s, t)));
            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(Activator.CreateInstance(type)));
            var sut = new ObjectBuilder(creator, converter, null);

            var constructionNode = new ConstructionNode(typeof(Collection))
            {
                Children = new List<ConstructionNode>()
                {
                    new ConstructionNode(typeof(int))
                    {
                        SourceValue = "1"
                    },
                    new ConstructionNode(typeof(int))
                    {
                        SourceValue = "2"
                    },
                    new ConstructionNode(typeof(int))
                    {
                        SourceValue = "3"
                    },
                }
            };

            var result = sut.Build(constructionNode);

            Assert.Equal((IEnumerable) result.Instance, new Collection() {1, 2, 3,});
        }

        [Fact]
        public void Assignment()
        {
            var textBlock = new TextBlock();
            var converter = new FuncStringConverterExtended((s, t) => (true, Convert.ChangeType(s, t)));

            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(textBlock));
            var sut = new ObjectBuilder(creator, converter, new FuncAssignmentApplier((assignment, i) =>
            {
                assignment.Member.SetValue(i, assignment.Children.First().Instance);
                return true;
            }));

            var constructionNode = new ConstructionNode(typeof(TextBlock))
            {
               Assignments = new List<MemberAssignment>()
               {
                   new MemberAssignment()
                   {
                       Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                       Children = ConstructionNode.ForString("SomeText"),
                   }
               }
            };

            sut.Build(constructionNode);

            Assert.Equal("SomeText", textBlock.Text);
        }
    }
}