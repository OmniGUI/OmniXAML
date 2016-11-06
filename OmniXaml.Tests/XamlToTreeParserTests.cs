namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Reflection;
    using DefaultLoader;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using TypeLocation;

    [TestClass]
    public class XamlToTreeParserTests
    {
        [TestMethod]
        public void ObjectAndDirectProperties()
        {
            var tree = Parse(@"<Window xmlns=""root"" Title=""Saludos"" />");
        }

        private static ConstructionNode Parse(string xaml)
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));
            var directory = new TypeDirectory();
            directory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));
            directory.AddNamespace(XamlNamespace.Map("root").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model")));
            directory.AddNamespace(XamlNamespace.Map("custom").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model.Custom")));

            var sut = new XamlToTreeParser(directory, new AttributeBasedMetadataProvider(), new[] { new InlineParser(directory) });

            var tree = sut.Parse(xaml);
            return tree;
        }

        [TestMethod]
        public void InnerStringProperty()
        {
            var tree = Parse(@"<Window xmlns=""root""><Window.Content>Hola</Window.Content></Window>");
        }

        [TestMethod]
        public void InnerComplexProperty()
        {
            var tree = Parse(@"<Window xmlns=""root""><Window.Content><TextBlock /></Window.Content></Window>");
        }

        [TestMethod]
        public void ImmutableFromContent()
        {
            var tree = Parse(@"<MyImmutable xmlns=""root"">hola</MyImmutable>");
        }

        [TestMethod]
        public void ContentPropertyDirectContent()
        {
            var tree = Parse(@"<Window xmlns=""root""><TextBlock /></Window>");
        }

        [TestMethod]
        public void ContentPropertyDirectContentText()
        {
            var tree = Parse(@"<TextBlock xmlns=""root"">Hello</TextBlock>");
        }

        [TestMethod]
        public void ContentPropertyDirectContentTextInsideChild()
        {
            var tree = Parse(@"<Window xmlns=""root""><TextBlock>Saludos cordiales</TextBlock></Window>");
        }

        [TestMethod]
        public void Namescope()
        {
            var actualNode = Parse(@"<Window xmlns:x=""special"" xmlns=""root"" ><TextBlock x:Name=""One"" /></Window>");
            var expectedNode = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<PropertyAssignment>()
                {
                    new PropertyAssignment()
                    {
                        Property = Property.FromStandard<Window>(w => w.Content),
                        Children = new List<ConstructionNode>()
                        {
                            new ConstructionNode(typeof(TextBlock))
                            {
                                Name = "One",
                                Assignments = new []{ new PropertyAssignment() { Property = Property.FromStandard<TextBlock>(block => block.Name), SourceValue = "One"} }
                            }
                        }
                    }
                }
            };

            Assert.AreEqual(expectedNode, actualNode);
        }

        [TestMethod]
        public void MarkupExtension()
        {
            var tree = Parse(@"<Window xmlns=""root"" Content=""{Simple}"" />");
        }

        [TestMethod]
        public void CData()
        {
            var tree = Parse(@"<Window xmlns=""root""><Window.Content><![CDATA[Hello]]></Window.Content></Window>");
        }

        [TestMethod]
        public void XmlNs()
        {
            var tree = Parse(@"<Window xmlns=""root"" xmlns:a=""custom"">
                                    <Window.Content>         
                                        <a:CustomControl />                           
                                    </Window.Content>
                                </Window>");
        }

        [TestMethod]
        public void AttachedPropertyFromAnotherNs()
        {
            var tree = Parse(@"<Window xmlns=""root"" xmlns:a=""custom"" a:CustomGrid.Value=""1"" />");
        }

        [TestMethod]
        public void ClrNs()
        {
            var tree = Parse(@"<Window xmlns=""using:OmniXaml.Tests.Model;Assembly=OmniXaml.Tests"" />");
        }

        [TestMethod]
        public void Name()
        {
            var tree = Parse(@"<Window xmlns=""root"" Name=""MyWindow"" />");
            Assert.AreEqual(new ConstructionNode(typeof(Window))
            {
                Name = "MyWindow",
                Assignments = new[] { new PropertyAssignment() { Property = Property.FromStandard<Window>(window => window.Name), SourceValue = "MyWindow" }, }
            }, tree);
        }

        [TestMethod]
        public void XName()
        {
            var tree = Parse(@"<Window xmlns=""root"" xmlns:x=""special"" x:Name=""MyWindow"" />");
            Assert.AreEqual(
                new ConstructionNode(typeof(Window))
                {
                    Name = "MyWindow",
                    Assignments = new[] { new PropertyAssignment() { Property = Property.FromStandard<Window>(window => window.Name), SourceValue = "MyWindow" }, }
                },
                tree);
        }

        [TestMethod]
        public void RegularEvent()
        {
            var tree = Parse(@"<Window xmlns=""root"">
                                <Button Click=""OnClick"" />
                               </Window>");
        }

        [TestMethod]
        public void AttachedEvent()
        {
            var tree = Parse(@"<Window xmlns=""root"" Window.Loaded=""OnLoad"" />");
        }
    }
}