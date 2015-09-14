namespace OmniXaml.Tests
{
    using System.Collections.ObjectModel;
    using Builder;
    using Classes;
    using Classes.Templates;
    using Classes.WpfLikeModel;
    using Common;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectAssembler;

    [TestClass]
    public class TemplateHostingObjectAssemblerTests : GivenAWiringContextWithNodeBuildersNetCore
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

            var mapping = new DeferredLoaderMapping();
            var assembler = new DummyDeferredLoader();
            mapping.Map<Template>(t => t.Content, assembler);

            var sut = new TemplateHostingObjectAssembler(new ObjectAssembler(WiringContext, new TopDownValueContext()), mapping);                       

            foreach (var instruction in input)
            {
                sut.Process(instruction);
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