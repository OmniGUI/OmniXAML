namespace OmniXaml.Tests.XmlParserTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using Model.Custom;

    [TestClass]
    public class StandardTests : XamlToTreeParserTestsBase
    {
        [TestMethod]
        public void ObjectAndDirectProperties()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"" Title=""Saludos"" />");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
               {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(window => window.Title),
                        SourceValue = "Saludos",
                    }
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        public void PropertyElementWithTextContent()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><Window.Content>Hola</Window.Content></Window>");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
               {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        SourceValue = "Hola",
                    }
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        public void PropertyElementWithChild()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><Window.Content><TextBlock /></Window.Content></Window>");


            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = new []{ new ConstructionNode(typeof(TextBlock)), }
                    }
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }
        
        [TestMethod]
        public void PropertyElementThatIsAnAttachedProperty()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><Grid.Row>1</Grid.Row></Window>");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        SourceValue = "1",
                        Member = Member.FromAttached<Grid>("Row"),
                    }
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        public void ImmutableFromContent()
        {
            var parseResult = ParseResult(@"<MyImmutable xmlns=""root"">hola</MyImmutable>");

            var expected = new ConstructionNode(typeof(MyImmutable)) {InjectableArguments = new[] {"hola"}};

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        public void ContentPropertyDirectContent()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><TextBlock /></Window>");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(tb => tb.Content),
                        Children = new[]
                        {
                            new ConstructionNode(typeof(TextBlock)),
                        }
                    },
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        public void ContentPropertyDirectContentText()
        {
            var parseResult = ParseResult(@"<TextBlock xmlns=""root"">Hello</TextBlock>");

            var expected = new ConstructionNode(typeof(TextBlock))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                        SourceValue = "Hello"
                    },
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        public void ContentPropertyDirectContentTextInsideChild()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><TextBlock>Saludos cordiales</TextBlock></Window>");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = new[]
                        {
                            new ConstructionNode(typeof(TextBlock))
                            {
                                Assignments = new[]
                                {
                                    new MemberAssignment()
                                    {
                                        Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                                        SourceValue = "Saludos cordiales"
                                    },
                                }
                            },
                        }
                    }
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }
        
        [TestMethod]
        public void MarkupExtensionWithoutPrefix()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"" Content=""{Simple}"" />");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = new[] {new ConstructionNode(typeof(SimpleExtension)),}
                    }
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeNotFoundException))]
        public void InlineMarkupExtension_ThatPointsTo_TypeNotImplementing_The_Correct_Interface()
        {
            ParseResult(@"<Window xmlns=""root"" Content=""{TextBlock}"" />");
        }

        [TestMethod]
        public void ChildFromPrefixedNamespace()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"" xmlns:a=""custom"">
                                    <Window.Content>         
                                        <a:CustomControl />                           
                                    </Window.Content>
                                </Window>");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
               {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = new []{ new ConstructionNode(typeof(CustomControl)), }
                    }
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        public void AttachedPropertyFromPrefixedNamespace()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root"" xmlns:a=""custom"" a:CustomGrid.Value=""1"" />");
            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        SourceValue = "1",
                        Member = Member.FromAttached<CustomGrid>("Value"),
                    }
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
        public void ClrNs()
        {
            var parseResult = ParseResult(@"<Window xmlns=""using:OmniXaml.Tests.Model;Assembly=OmniXaml.Tests"" />");

            var expected = new ConstructionNode(typeof(Window));

            Assert.AreEqual(expected, parseResult.Root);
        }        
    }
}