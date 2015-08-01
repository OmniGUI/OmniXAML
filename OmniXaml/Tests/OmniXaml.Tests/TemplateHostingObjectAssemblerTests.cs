namespace OmniXaml.Tests
{
    using System.Collections.ObjectModel;
    using Builder;
    using Classes;
    using Classes.Templates;
    using Classes.WpfLikeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;

    [TestClass]
    public class TemplateHostingObjectAssemblerTests : GivenAWiringContext
    {
        private XamlNodeBuilder x;

        public TemplateHostingObjectAssemblerTests()
        {
            x = new XamlNodeBuilder(WiringContext.TypeContext);
        }

        [TestMethod]
        public void SimpleTest()
        {
            var input = new Collection<XamlNode>
            {
                x.StartObject<Item>(),
                x.StartMember<Item>(i => i.Template),
                x.StartObject<Template>(),
                x.StartMember<Template>(i => i.Content),
                x.StartObject<Grid>(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
            };

            var sut = new TemplateHostingObjectAssembler(new SuperObjectAssembler(WiringContext, new TopDownMemberValueContext()));

            var assembler = new DummyDeferredLoader();
            sut.AddDeferredLoader<Template>(t => t.Content, assembler);

            foreach (var xamlNode in input)
            {
                sut.Process(xamlNode);
            }

            var actualNodes = sut.NodeList;
            var expectedNodes = new Collection<XamlNode>
            {
                x.StartObject<Grid>(),
                x.EndObject(),                
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
            Assert.IsNotNull(((Item) sut.Result).Template.Content);
        }
    }    
}