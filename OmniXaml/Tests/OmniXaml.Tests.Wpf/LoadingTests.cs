namespace OmniXaml.Tests.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Resources = Xaml.Tests.Resources.Wpf;

    [TestClass]
    public class LoadingTests : GivenAXamlXmlLoader
    {
        [TestMethod]
        public void Window()
        {
            var windowType = typeof(Window);           

            var actualInstance = LoadXaml(Resources.Window);
            Assert.IsInstanceOfType(actualInstance, windowType);
        }

        [TestMethod]
        public void WindowWithContent()
        {
            var windowType = typeof(Window);
            var textBlockType = typeof(TextBlock);

            var actualInstance = LoadXaml(Resources.WindowWithContent);
            Assert.IsInstanceOfType(actualInstance, windowType);
            var window = (Window)actualInstance;
            Assert.IsInstanceOfType(window.Content, textBlockType);
            var textBlock = (TextBlock)window.Content;
            Assert.AreEqual("Saludos cordiales!", textBlock.Text);
        }

        [TestMethod]
        [Ignore]
        public void BindingTest()
        {
            var visualTree = LoadXaml(Resources.DataTemplate);            
        }

        [TestMethod]

        public void ShowCaseTest()
        {
            var visualTree = LoadXaml(Resources.ShowCase);
        }
    }
}
