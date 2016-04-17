namespace OmniXaml.Tests
{
    using System.Collections.ObjectModel;
    using Classes;
    using Common.DotNetFx;
    using Xunit;
    using Visualization;

    public class VisualizationTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {     
        [Fact]
        public void ConvertToTags()
        {
            var col = new Collection<Instruction>()
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.StartMember<DummyClass>(@class => @class.Child),
                X.StartObject<ChildClass>(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var result = NodeVisualizer.ToTags(col);
        }

        [Fact]
        public void ConvertToNodes()
        {
            var col = new Collection<Instruction>()
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.StartMember<DummyClass>(@class => @class.Child),
                X.StartObject<ChildClass>(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var result = NodeVisualizer.ToTree(col);
        }
    }
}