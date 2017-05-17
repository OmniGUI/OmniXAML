using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniXaml.ReworkPhases;
using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests
{
    public class ObjectAssemblerTests
    {
        [Fact]
        public void SourceValue()
        {
            var converter = new FuncStringConverterExtended((s, t) => (true, Convert.ChangeType(s, t)));
            var sut = new ObjectAssembler(null, converter, null);
            var constructionNode = new ConstructionNode(typeof(int)) {SourceValue = "1"};
            sut.Assemble(constructionNode);
            Assert.Equal(1, constructionNode.Instance);
        }

        [Fact]
        public void NodeWithOnlyATypeShouldReturnTheProvidedInstanced()
        {
            var someInstance = new TextBlock();
            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(someInstance));
            var sut = new ObjectAssembler(creator, null, null);

            var constructionNode = new ConstructionNode(typeof(TextBlock));
            sut.Assemble(constructionNode);

            Assert.Equal(someInstance, constructionNode.Instance);
        }

        [Fact]
        public void NodeWithChildrenShouldGetThemAsChildren()
        {
            var converter = new FuncStringConverterExtended((s, t) => (true, Convert.ChangeType(s, t)));
            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(Activator.CreateInstance(type)));
            var sut = new ObjectAssembler(creator, converter, null);

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

            sut.Assemble(constructionNode);

            Assert.Equal(new Collection() {1, 2, 3,}, (IEnumerable)constructionNode.Instance);
        }

        [Fact]
        public void ConstructionWithAssignmentShouldExecuteAssigmentApplier()
        {
            var textBlock = new TextBlock();
            var converter = new FuncStringConverterExtended((s, t) => (true, Convert.ChangeType(s, t)));

            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(textBlock));
            var sut = new ObjectAssembler(creator, converter, new FuncAssignmentApplier((assignment, i) =>
            {
                assignment.Member.SetValue(i, assignment.Values.First().Instance);
            }));

            var constructionNode = new ConstructionNode(typeof(TextBlock))
            {
               Assignments = new List<MemberAssignment>()
               {
                   new MemberAssignment()
                   {
                       Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                       SourceValue = "SomeText",
                   }
               }
            };

            sut.Assemble(constructionNode);

            Assert.Equal("SomeText", textBlock.Text);
        }
    }
}