namespace OmniXaml.AppServices.NetCore.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Tests.Classes;
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using OmniXaml.Tests.Common;

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

        private static InflatableTypeFactory CreateSut()
        {
            var inflatableTypeFactory = new InflatableTypeFactory(
                new TypeFactory(),
                new InflatableTranslator(),
                typeFactory => new DefaultXamlLoader(new DummyWiringContext(typeFactory, Assemblies.AssembliesInAppFolder)))
            {
                Inflatables = new Collection<Type> {typeof (Window), typeof (UserControl)},
            };

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
