namespace OmniXaml.Tests.XmlParserTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Model;
    using Services;
    using Xunit;
    
    public class AssignmentExtractorTests
    {
        [Fact]
        public void ContentPropertyWithChildren()
        {
            var assigments = Parse(@"<ItemsControl xmlns=""root""><TextBlock/><TextBlock/><TextBlock/></ItemsControl>", (e, a) => new ConstructionNode(typeof(TextBlock)), typeof(ItemsControl));

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

            Assert.Equal(expected, assigments);
        }

        [Fact]
        public void ContentPropertyText()
        {
            var assigments = Parse(@"<TextBlock xmlns=""root"">Hola</TextBlock>", (e, a) => new ConstructionNode(typeof(TextBlock)), typeof(TextBlock));

            var expected = new[]
            {
                new MemberAssignment
                {
                    Member = Member.FromStandard<TextBlock>(collection => collection.Text),
                    SourceValue = "Hola",
                }
            };

            Assert.Equal(expected, assigments);
        }

        [Fact]
        public void ElementOrdering_InvalidPropertyElementOrder()
        {
            var xaml = @"<ItemsControl xmlns=""root"">
<TextBlock/>
<ItemsControl.HeaderText>Hola</ItemsControl.HeaderText>
<TextBlock/>
</ItemsControl>";

            Assert.Throws<ParseException>(() => Parse(xaml, (element, annotator) => new ConstructionNode(typeof(TextBlock)), typeof(ItemsControl)));
        }

        [Fact]
        public void ElementOrdering_PropertyAfterDirectContent()
        {
            var xaml = @"<ItemsControl xmlns=""root"">
<TextBlock/>
<TextBlock/>
<ItemsControl.HeaderText>Hola</ItemsControl.HeaderText>
</ItemsControl>";

            Parse(xaml, (element, annotator) => new ConstructionNode(typeof(TextBlock)), typeof(ItemsControl));
        }

        [Fact]
        public void ElementOrdering_PropertyBeforeDirectContent()
        {
            var xaml = @"<ItemsControl xmlns=""root"">
<ItemsControl.HeaderText>Hola</ItemsControl.HeaderText>
<TextBlock/>
<TextBlock/>
</ItemsControl>";

            Parse(xaml, (element, annotator) => new ConstructionNode(typeof(TextBlock)), typeof(ItemsControl));
        }

        private static List<MemberAssignment> Parse(string xaml, Func<XElement, IPrefixAnnotator, ConstructionNode> parser, Type type)
        {
            var typeDirectory = new AttributeBasedTypeDirectory(new List<Assembly>() { Assembly.GetExecutingAssembly() });
            var sut = new AssignmentExtractor(new AttributeBasedMetadataProvider(), new InlineParser[0], new Resolver(typeDirectory), parser);

            var assigments = sut.GetAssignments(type, XElement.Parse(xaml), new PrefixAnnotator()).ToList();
            return assigments;
        }

        [Fact]
        public void PropertyElement()
        {
            var xaml =
@"<ItemsControl xmlns=""root"">
    <ItemsControl.HeaderText>Hola</ItemsControl.HeaderText>
</ItemsControl>";

            var constructionNode = new ConstructionNode(typeof(string));
            var assigments = Parse(xaml, (element, annotator) => constructionNode, typeof(ItemsControl));

            var expected = new[]
            {
                new MemberAssignment
                {
                    Member = Member.FromStandard<ItemsControl>(collection => collection.HeaderText),
                    SourceValue = "Hola",
                }
            };

            Assert.Equal(expected, assigments);
        }
    }    
}