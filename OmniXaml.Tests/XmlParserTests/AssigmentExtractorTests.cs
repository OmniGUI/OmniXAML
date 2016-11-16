namespace OmniXaml.Tests.XmlParserTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using DefaultLoader;
    using Metadata;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class AssigmentExtractorTests
    {
        [TestMethod]
        public void ContentPropertyWithChildren()
        {
            var assigments = Parse(@"<ItemsControl xmlns=""root""><TextBlock/><TextBlock/><TextBlock/></ItemsControl>", element => new ConstructionNode(typeof(TextBlock)));

            var expected = new[]
            {
                new MemberAssignment
                {
                    Member = Member.FromStandard<ItemsControl>(collection => collection.Items),
                    Children = new[]
                    {
                        new ConstructionNode(typeof(TextBlock)),
                        new ConstructionNode(typeof(TextBlock)),
                        new ConstructionNode(typeof(TextBlock)),
                    }
                }
            };

            CollectionAssert.AreEqual(expected, assigments);
        }

        private static List<MemberAssignment> Parse(string xaml, Func<object, ConstructionNode> parser)
        {
            var typeDirectory = new AttributeBasedTypeDirectory(new List<Assembly>() { Assembly.GetExecutingAssembly() });
            var sut = new AssignmentExtractor(new AttributeBasedMetadataProvider(), new InlineParser[0], new Resolver(typeDirectory), parser);

            var assigments = sut.GetAssignments(typeof(ItemsControl), XElement.Parse(xaml)).ToList();
            return assigments;
        }

        [TestMethod]
        public void PropertyElement()
        {
            var xaml =
@"<ItemsControl xmlns=""root"">
    <ItemsControl.HeaderText>Hola</ItemsControl.HeaderText>
</ItemsControl>";

            var constructionNode = new ConstructionNode(typeof(string));
            var assigments = Parse(xaml, element =>
             {
                 return constructionNode;
             });

            var expected = new[]
            {
                new MemberAssignment
                {
                    Member = Member.FromStandard<ItemsControl>(collection => collection.HeaderText),
                    SourceValue = "Hola",
                }
            };

            CollectionAssert.AreEqual(expected, assigments);
        }
    }

    
}