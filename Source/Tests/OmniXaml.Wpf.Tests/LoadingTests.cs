namespace OmniXaml.Wpf.Tests
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
        public void DataTemplate()
        {
            var visualTree = LoadXaml(Resources.DataTemplate);            
        }

        [TestMethod]
        public void ShowCase()
        {
            var visualTree = LoadXaml(Resources.ShowCase);
        }

        [TestMethod]

        public void MicroShowCase()
        {
            var visualTree = LoadXaml(Resources.MicroShowCase);
        }

        [TestMethod]

        public void Stage1()
        {
            var visualTree = LoadXaml(Resources.Stage1);
        }

        public void Stage2()
        {
            var visualTree = LoadXaml(Resources.Stage2);
        }

        public void Stage3()
        {
            var visualTree = LoadXaml(Resources.Stage3);
        }

        [TestMethod]
        public void ColorResource()
        {
            var visualTree = LoadXaml(Resources.ColorResource);
        }
    }
}
