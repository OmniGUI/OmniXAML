using System.Collections.Generic;
using System.Reflection;
using OmniXaml.Services;
using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests.Integration
{
    public class XamlTests
    {
        [Fact]
        public void SingleInstance()
        {
            var xaml = @"<Window xmlns=""root"" />";
            var instance = LoadXaml(xaml);
            Assert.Equal(new Window(), instance);
        }

        [Fact]
        public void Property()
        {
            var xaml = @"<Window xmlns=""root"" Title=""Some title"" />";
            var instance = LoadXaml(xaml);
            Assert.Equal(new Window() { Title = "Some title"}, instance);
        }

        [Fact]
        public void Content()
        {
            var xaml = @"<Window xmlns=""root""><TextBlock Text=""Hello"" /></Window>";
            var instance = LoadXaml(xaml);
            Assert.Equal(new Window() {Content = new TextBlock() {Text = "Hello"}}, instance);
        }

        [Fact]
        public void Name()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:x=""special"" x:Name=""MyWindow"" />";
            var instance = LoadXaml(xaml);
            Assert.Equal(new Window() { Name = "MyWindow" }, instance);
        }

        [Fact]
        public void Collection()
        {
            var xaml = @"<Collection xmlns=""root"">
<TextBlock Text=""One""/>
<TextBlock Text=""Two""/>
</Collection>";
            var instance = LoadXaml(xaml);
            var expected = new Collection() {new TextBlock() {Text = "One"}, new TextBlock() {Text = "Two"}};
            Assert.Equal(expected, instance);
        }

        [Fact]
        public void Extension()
        {
            var xaml = @"<TextBlock xmlns=""root"" Text=""{SimpleExtension Property=Hello}""/>";
            var instance = LoadXaml(xaml);
            var expected = new TextBlock() {Text = "Hello"};
            Assert.Equal(expected, instance);
        }

        private static object LoadXaml(string xaml)
        {
            var loader = new ExtendedXamlLoader(new List<Assembly> {typeof(Window).GetTypeInfo().Assembly});
            return loader.Load(xaml);
        }
    }
}
