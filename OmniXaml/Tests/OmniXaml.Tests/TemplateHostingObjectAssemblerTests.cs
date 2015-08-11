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
    public class TemplateHostingObjectAssemblerTests : GivenAWiringContextWithNodeBuilders
    {
        private XamlInstructionBuilder x;

        public TemplateHostingObjectAssemblerTests()
        {
            x = new XamlInstructionBuilder(WiringContext.TypeContext);
        }

        [TestMethod]
        public void SimpleTest()
        {
            var input = new Collection<XamlInstruction>
            {
                X.StartObject<Item>(),
                X.StartMember<Item>(i => i.Template),
                X.StartObject<Template>(),
                X.StartMember<Template>(i => i.Content),
                X.StartObject<Grid>(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var sut = new TemplateHostingObjectAssembler(new SuperObjectAssembler(WiringContext, new TopDownMemberValueContext()));

            var assembler = new DummyDeferredLoader();
            sut.AddDeferredLoader<Template>(t => t.Content, assembler);

            foreach (var xamlNode in input)
            {
                sut.Process(xamlNode);
            }

            var actualNodes = sut.NodeList;
            var expectedInstructions = new Collection<XamlInstruction>
            {
                X.StartObject<Grid>(),
                X.EndObject(),                
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
            Assert.IsNotNull(((Item) sut.Result).Template.Content);
        }
    }    
}