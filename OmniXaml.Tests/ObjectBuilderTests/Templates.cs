namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using Model;
    using Xunit;

    public class Templates : ObjectBuilderTestsBase
    {
        [Fact]
        public void TemplateContent()
        {
            var node = new ConstructionNode(typeof(ItemsControl))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<ItemsControl>(control => control.ItemTemplate),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode(typeof(DataTemplate))
                            {
                                Assignments = new[]
                                {
                                    new MemberAssignment
                                    {
                                        Member = Member.FromStandard<DataTemplate>(template => template.Content),
                                        Children = new[] {new ConstructionNode(typeof(TextBlock))}
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var creationFixture = Create(node);
        }
    }
}