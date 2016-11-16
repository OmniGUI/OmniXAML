namespace OmniXaml.Tests.XmlParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using DefaultLoader;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class AssigmentExtractorTests
    {
        [TestMethod]
        public void Test()
        {
            var attributeBasedTypeDirectory = new AttributeBasedTypeDirectory(new List<Assembly>() { Assembly.GetExecutingAssembly() });
            var sut = new AssignmentExtractor(new AttributeBasedMetadataProvider(), attributeBasedTypeDirectory, new InlineParser[0], element => new ConstructionNode(typeof(TextBlock)));
            var assigments = sut.GetAssignments(typeof(ItemsControl), XElement.Parse(@"<ItemsControl xmlns=""root""><TextBlock/><TextBlock/><TextBlock/></ItemsControl>")).ToList();
        }
    }
}