using System.Collections.Generic;
using OmniXaml.Tests.Model;
using OmniXaml.Tests.Model.Custom;
using Xunit;

namespace OmniXaml.Tests.XmlToNodes
{
    public class ClassDirective : XamlToTreeParserTestsBase
    {
        [Fact]
        public void Class()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:x=""special"" x:Class=""OmniXaml.Tests.Model.Custom.CustomWindow;assembly=OmniXaml.Tests"" />";
            var p = ParseResult(xaml);
            ConstructionNode expected = new ConstructionNode<Window> { InstantiateAs = typeof(CustomWindow) };

            Assert.Equal(expected, p.Root);
        }

        [Fact]
        public void EventAttachedToCorrectInstance()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:x=""special"" x:Class=""OmniXaml.Tests.Model.TestWindow;assembly=OmniXaml.Tests"" Clicked=""OnClick"" />";
            var p = ParseResult(xaml);
            
            var expected = new ConstructionNode<Window, TestWindow>()
                .WithAssignment("Clicked", "OnClick");

            Assert.Equal(expected, p.Root);
        }
    }
}