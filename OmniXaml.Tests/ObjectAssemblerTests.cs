using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var sut = new NodeAssembler(null, converter, null);
            var constructionNode = new ConstructionNode<int> {SourceValue = "1"};
            sut.Assemble(constructionNode, null);
            Assert.Equal(1, constructionNode.Instance);
        }

        [Fact]
        public void NodeWithOnlyATypeShouldReturnTheProvidedInstanced()
        {
            var someInstance = new TextBlock();
            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(someInstance));
            var sut = new NodeAssembler(creator, null, null);

            var constructionNode = new ConstructionNode<TextBlock>();
            sut.Assemble(constructionNode, null);

            Assert.Equal(someInstance, constructionNode.Instance);
        }

        [Fact]
        public void NodeWithChildrenShouldGetThemAsChildren()
        {
            var converter = new FuncStringConverterExtended((s, t) => (true, Convert.ChangeType(s, t)));
            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(Activator.CreateInstance(type)));
            var sut = new NodeAssembler(creator, converter, null);

            var constructionNode = new ConstructionNode<Collection>()
                .WithChildren(
                    new ConstructionNode<int>
                    {
                        SourceValue = "1"
                    },
                    new ConstructionNode<int>
                    {
                        SourceValue = "2"
                    },
                    new ConstructionNode<int>
                    {
                        SourceValue = "3"
                    }
                );

            sut.Assemble(constructionNode, null);

            Assert.Equal(new Collection() {1, 2, 3,}, (IEnumerable)constructionNode.Instance);
        }

        [Fact]
        public void ConstructionWithAssignmentShouldExecuteAssigmentApplier()
        {
            var textBlock = new TextBlock();
            var converter = new FuncStringConverterExtended((s, t) => (true, Convert.ChangeType(s, t)));

            var creator = new FuncInstanceCreator((hints, type) => new CreationResult(textBlock));
            var sut = new NodeAssembler(creator, converter, new FuncAssignmentApplier((assignment, objectBuilder, context) =>
            {
                assignment.Assignment.Member.SetValue(assignment.Instance, assignment.Assignment.Values.First().Instance);
            }));

            var constructionNode = new ConstructionNode<TextBlock>().WithAssignment(tb => tb.Text, "SomeText");

            sut.Assemble(constructionNode, null);

            Assert.Equal("SomeText", textBlock.Text);
        }
    }
}