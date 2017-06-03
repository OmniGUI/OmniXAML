using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using OmniXaml.Services;
using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests.XmlToNodes
{
    public class AssignmentExtractorTests
    {
        private static List<MemberAssignment> Parse(string xaml,
            Func<XElement, IPrefixAnnotator, ConstructionNode> parser, Type type)
        {
            var typeDirectory = new AttributeBasedTypeDirectory(new List<Assembly> {typeof(ModelObject).GetTypeInfo().Assembly});
            var sut = new AssignmentExtractor(new AttributeBasedMetadataProvider(), new InlineParser[0],
                new XmlTypeXmlTypeResolver(typeDirectory), parser);

            var assigments = sut.GetAssignments(type, XElement.Parse(xaml), new PrefixAnnotator()).ToList();
            return assigments;
        }

        [Fact]
        public void ContentPropertyText()
        {
            var assigments = Parse(@"<TextBlock xmlns=""root"">Hola</TextBlock>",
                (e, a) => new ConstructionNode<TextBlock>(), typeof(TextBlock));

            var expected = new[]
            {
                new MemberAssignment<TextBlock>(tb => tb.Text, "Hola")
            };

            Assert.Equal(expected, assigments);
        }

        [Fact]
        public void ContentPropertyWithChildren()
        {
            var assigments = Parse(@"<ItemsControl xmlns=""root""><TextBlock/><TextBlock/><TextBlock/></ItemsControl>",
                (e, a) => new ConstructionNode<TextBlock>(), typeof(ItemsControl));

            var expected = new[]
            {
                new MemberAssignment<ItemsControl>(i => i.Items,
                    new ConstructionNode<TextBlock>(),
                    new ConstructionNode<TextBlock>(),
                    new ConstructionNode<TextBlock>())
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

            Assert.Throws<ParseException>(() => Parse(xaml,
                (element, annotator) => new ConstructionNode<TextBlock>(), typeof(ItemsControl)));
        }

        [Fact]
        public void ElementOrdering_PropertyAfterDirectContent()
        {
            var xaml = @"<ItemsControl xmlns=""root"">
<TextBlock/>
<TextBlock/>
<ItemsControl.HeaderText>Hola</ItemsControl.HeaderText>
</ItemsControl>";

            Parse(xaml, (element, annotator) => new ConstructionNode<TextBlock>(), typeof(ItemsControl));
        }

        [Fact]
        public void ElementOrdering_PropertyBeforeDirectContent()
        {
            var xaml = @"<ItemsControl xmlns=""root"">
<ItemsControl.HeaderText>Hola</ItemsControl.HeaderText>
<TextBlock/>
<TextBlock/>
</ItemsControl>";

            Parse(xaml, (element, annotator) => new ConstructionNode<TextBlock>(), typeof(ItemsControl));
        }

        [Fact]
        public void PropertyElement()
        {
            var xaml =
                @"<ItemsControl xmlns=""root"">
    <ItemsControl.HeaderText>Hola</ItemsControl.HeaderText>
</ItemsControl>";

            var constructionNode = new ConstructionNode<string>();
            var assigments = Parse(xaml, (element, annotator) => constructionNode, typeof(ItemsControl));

            var expected = new[]
            {
                new MemberAssignment
                {
                    Member = Member.FromStandard<ItemsControl>(collection => collection.HeaderText),
                    SourceValue = "Hola"
                }
            };

            Assert.Equal(expected, assigments);
        }
    }
}