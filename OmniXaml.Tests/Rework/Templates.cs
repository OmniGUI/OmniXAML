namespace OmniXaml.Tests.Rework
{
    using System.Collections.Generic;
    using Model;
    using ObjectBuilderTests;
    using OmniXaml.Rework;
    using Xunit;

    public class Templates
    {
        [Fact(Skip = "No furrulla")]
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

        private static object Create(ConstructionNode cn)
        {
            var valuePipeline = new PipelineMock();
            valuePipeline.SetMutator((parent, member, mut) =>
            {
                var me = mut.Value as IMarkupExtension;
                if (me != null)
                {
                    mut.Value = me.GetValue(null);
                }
            });

            return new NewObjectBuilder(new SmartInstanceCreatorMock(), new SmartConverterMock(), valuePipeline).Inflate(cn);
        }
    }
}