namespace OmniXaml.AppServices.NetCore.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using AppServices.Tests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Tests.Classes;
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using OmniXaml.Tests.Common;
    using OmniXaml.Tests.Common.NetCore;
    using Services;
    using Services.DotNetFx;

    [TestClass]
    public class InflatableFactoryTest
    {
        [TestMethod]
        public void Window()
        {
            var sut = CreateSut();

            var myWindow = sut.Create<MyWindow>();
            Assert.IsInstanceOfType(myWindow, typeof(MyWindow));
            Assert.AreEqual(myWindow.Title, "Hello World :)");
        }

        private static AutoInflatingTypeFactory CreateSut()
        {
            var inflatableTypeFactory = new DummyAutoInflatingTypeFactory(
                new TypeFactory(),
                new InflatableTranslator(),
                typeFactory => new XamlLoader(new DummyXamlParserFactory(new DummyWiringContext(typeFactory, Assemblies.AssembliesInAppFolder))));

            return inflatableTypeFactory;
        }

        [TestMethod]
        public void UserControl()
        {
            var sut = CreateSut();

            var myWindow = sut.Create<WindowWithUserControl>();
            Assert.IsInstanceOfType(myWindow, typeof(WindowWithUserControl));
            Assert.AreEqual(myWindow.Title, "Hello World :)");
            Assert.IsInstanceOfType(myWindow.Content, typeof(UserControl));
            Assert.AreEqual("It's-a me, Mario", ((UserControl)myWindow.Content).Property);
        }

        [TestMethod]
        public void UserControlLoadingWithUri()
        {
            var sut = CreateSut();

            var myWindow = (Window)sut.Create(new Uri("WpfLikeModel/WindowWithUserControl.xaml", UriKind.Relative));
            Assert.IsInstanceOfType(myWindow, typeof(WindowWithUserControl));
            Assert.AreEqual(myWindow.Title, "Hello World :)");
            Assert.IsInstanceOfType(myWindow.Content, typeof(UserControl));
            var userControl = ((UserControl)myWindow.Content);
            Assert.AreEqual("It's-a me, Mario", userControl.Property);
            Assert.IsInstanceOfType(userControl.Content, typeof(ChildClass));
        }
    }
}
