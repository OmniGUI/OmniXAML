namespace OmniXaml.Tests.XmlParser
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EventsTests : XamlToTreeParserTestsBase
    {
        [TestMethod]
        public void BasicEvent()
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