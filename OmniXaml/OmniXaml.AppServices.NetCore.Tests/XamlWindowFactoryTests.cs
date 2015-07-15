using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OmniXaml.AppServices.NetCore.Tests
{
    using TestApplication;
    using TestModel;
    using Wpf;

    [TestClass]
    public class XamlWindowFactoryTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var fliping = new NetCoreResourceProvider();
            var p = fliping.GetStream(new Uri("Test.txt", UriKind.Relative));
        }

        [TestMethod]
        public void Load()
        {   
            var sut = new XamlWindowFactory(new WpfXamlLoader(), new TypeFactory(), new NetCoreResourceProvider(), new NetCoreXamlByTypeProvider());
            var myWindow = sut.Create<MyWindow>();
            Assert.IsInstanceOfType(myWindow, typeof(MyWindow));
            Assert.AreEqual(myWindow.Title, "Hello World :)");
        }
    }
}
