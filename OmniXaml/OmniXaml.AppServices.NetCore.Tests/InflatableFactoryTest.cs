namespace OmniXaml.AppServices.NetCore.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Tests.Classes;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

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
            var inflatableTypeFactory = new InflatableTypeFactory(new TypeFactory(), new NetCoreResourceProvider(), new NetCoreTypeToUriLocator(), GetLoader)
            {
                Inflatables = new Collection<Type> {typeof (Window), typeof (UserControl)},
            };

            return inflatableTypeFactory;
        }        

        private static IXamlStreamLoader GetLoader(InflatableTypeFactory typeFactory)
        {
            var context = DummyWiringContext.Create(typeFactory);
            Func<IObjectAssembler, ICoreXamlLoader> coreLoaderFactory =
                objectAssembler => new CoreXamlXmlLoader(new SuperProtoParser(context), new XamlNodesPullParser(context), objectAssembler);

            return new BootstrappableXamlStreamLoader(coreLoaderFactory, new DefaultObjectAssemblerFactory(context));
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

            var myWindow = (Window)sut.Create(new Uri("WindowWithUserControl.xaml", UriKind.Relative));
            Assert.IsInstanceOfType(myWindow, typeof(WindowWithUserControl));
            Assert.AreEqual(myWindow.Title, "Hello World :)");
            Assert.IsInstanceOfType(myWindow.Content, typeof(UserControl));
            var userControl = ((UserControl)myWindow.Content);
            Assert.AreEqual("It's-a me, Mario", userControl.Property);
            Assert.IsInstanceOfType(userControl.Content, typeof(ChildClass));
        }
    }
}
