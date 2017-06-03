using System.Xml;
using OmniXaml.Tests.Model;
using OmniXaml.Tests.Model.Custom;
using OmniXaml.TypeLocation;
using Xunit;

namespace OmniXaml.Tests.XmlToNodes
{
    public class StandardTests : XamlToTreeParserTestsBase
    {
        [Fact]
        public void NoDeclaredPrefixes()
        {
            Assert.Throws<TypeLocationException>(() => ParseResult(@"<Window />"));
        }

        [Fact]
        public void EmptyDefaultPrefix()
        {
            Assert.Throws<TypeLocationException>(() => ParseResult(@"<Window xmlns="""" />"));
        }

        [Fact]
        public void UndeclaredPrefix()
        {
            Assert.Throws<XmlException>(() => ParseResult(@"<u:Window />"));
        }

        [Fact]
        public void ObjectAndDirectProperties()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"" Title=""Saludos"" />");

            var expected = new ConstructionNode<Window>
            {
            }.WithAssignments(new[]
            {
                new MemberAssignment
                {
                    Member = Member.FromStandard<Window>(window => window.Title),
                    SourceValue = "Saludos",
                }
            });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void PropertyElementWithTextContent()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><Window.Content>Hola</Window.Content></Window>");

            var expected = new ConstructionNode<Window>().WithAssignments(new[]
            {
                new MemberAssignment()
                {
                    Member = Member.FromStandard<Window>(window => window.Content),
                    SourceValue = "Hola"
                }
            });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void PropertyElementWithChild()
        {
            var parseResult =
                ParseResult(@"<Window xmlns=""root""><Window.Content><TextBlock /></Window.Content></Window>");


            var expected = new ConstructionNode<Window>
            {
            }.WithAssignments(new[]
            {
                new MemberAssignment()
                {
                    Member = Member.FromStandard<Window>(window => window.Content),
                    Values = new[] {new ConstructionNode(typeof(TextBlock)),}
                }
            });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void PropertyElementThatIsAnAttachedProperty()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><Grid.Row>1</Grid.Row></Window>");

            var expected = new ConstructionNode<Window>
            {
            }.WithAssignments(new[]
            {
                new MemberAssignment()
                {

                    Member = Member.FromAttached<Grid>("Row"),
                    SourceValue = "1",
                }
            });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void PropertyElementThatIsACollectionInsideAttachedProperty()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"">
<VisualStateManager.VisualStateGroups>
<VisualStateGroup />
</VisualStateManager.VisualStateGroups>
</Window>");

            var expected = new ConstructionNode<Window>
            {
            }.WithAssignments(new[]
            {
                new MemberAssignment
                {
                    Values = new[] {new ConstructionNode(typeof(VisualStateGroup)),},
                    Member = Member.FromAttached<VisualStateManager>("VisualStateGroups"),
                }
            });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void ImmutableFromContent()
        {
            var parseResult = ParseResult(@"<MyImmutable xmlns=""root"">hola</MyImmutable>");

            var expected = new ConstructionNode(typeof(MyImmutable)) {PositionalParameters = new[] {"hola"}};

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void ContentPropertyDirectContent()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><TextBlock /></Window>");

            var expected = new ConstructionNode<Window>().WithAssignments(new[]
            {
                new MemberAssignment()
                {
                    Member = Member.FromStandard<Window>(tb => tb.Content),
                    Values = new[]
                    {
                        new ConstructionNode(typeof(TextBlock)),
                    }
                },
            });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void ContentPropertyDirectContentText()
        {
            var parseResult = ParseResult(@"<TextBlock xmlns=""root"">Hello</TextBlock>");

            var expected = new ConstructionNode(typeof(TextBlock))
            {
            }.WithAssignments(new[]
            {
                new MemberAssignment()
                {
                    Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                    SourceValue = "Hello"
                },
            });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void ContentPropertyDirectContentTextInsideChild()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><TextBlock>Saludos cordiales</TextBlock></Window>");

            var expected = new ConstructionNode<Window>()
                .WithAssignments(new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Values = new[]
                        {
                            new ConstructionNode(typeof(TextBlock))
                            {
                            }.WithAssignments(new[]
                            {
                                new MemberAssignment()
                                {
                                    Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                                    SourceValue = "Saludos cordiales",
                                },
                            }),
                        }
                    }
                });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void MarkupExtensionWithoutPrefix()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"" Content=""{Simple}"" />");

            var expected = new ConstructionNode<Window>()
                .WithAssignments(new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Values = new[] {new ConstructionNode(typeof(SimpleExtension)),}
                    }
                });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void InlineMarkupExtension_ThatPointsTo_TypeNotImplementing_The_Correct_Interface()
        {
            Assert.Throws<TypeNotFoundException>(
                () => ParseResult(@"<Window xmlns=""root"" Content=""{TextBlock}"" />"));
        }

        [Fact]
        public void ChildFromPrefixedNamespace()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"" xmlns:a=""custom"">
                                    <Window.Content>         
                                        <a:CustomControl />                           
                                    </Window.Content>
                                </Window>");

            var expected = new ConstructionNode<Window>()
                .WithAssignments(new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Values = new[] {new ConstructionNode(typeof(CustomControl)),}
                    }
                });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void AttachedPropertyFromPrefixedNamespace()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"" xmlns:a=""custom"" a:CustomGrid.Value=""1"" />");
            var expected = new ConstructionNode<Window>()
                .WithAssignments(new[]
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromAttached<CustomGrid>("Value"),
                        SourceValue = "1",
                    }
                });

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void ClrNs()
        {
            var parseResult = ParseResult(@"<Window xmlns=""using:OmniXaml.Tests.Model;Assembly=OmniXaml.Tests"" />");

            var expected = new ConstructionNode<Window>();

            Assert.Equal(expected, parseResult.Root);
        }
    }
}