namespace OmniXaml.Services.DotNetFx.Tests
{
    using System;
    using Xunit;
    using OmniXaml.Tests.Classes;
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using OmniXaml.Tests.Common.DotNetFx;
    using Services;
    using Services.DotNetFx;
    using Services.Tests;

    public class InflatableFactoryTest
    {
        [Fact(Skip = "Ignore")]
        public void Window()
        {
            var sut = CreateSut();

            var myWindow = sut.Create<MyWindow>();
            Assert.IsType(typeof (MyWindow), myWindow);
            Assert.Equal(myWindow.Title, "Hello World :)");
        }

        private static AutoInflatingTypeFactory CreateSut()
        {
            var inflatableTypeFactory = new DummyAutoInflatingTypeFactory(
                new TypeFactory(),
                new InflatableTranslator(),
                typeFactory => new XmlLoader(new DummyParserFactory(RuntimeTypeSource.FromAttributes(Assemblies.AssembliesInAppFolder))));

            return inflatableTypeFactory;
        }

        [Fact(Skip = "Ignore")]
        public void UserControl()
        {
            var sut = CreateSut();

            var myWindow = sut.Create<WindowWithUserControl>();
            Assert.IsType(typeof (WindowWithUserControl), myWindow);
            Assert.Equal(myWindow.Title, "Hello World :)");
            Assert.IsType(typeof (UserControl), myWindow.Content);
            Assert.Equal("It's-a me, Mario", ((UserControl) myWindow.Content).Property);
        }

        [Fact(Skip = "Ignore")]
        public void UserControlLoadingWithUri()
        {
            var sut = CreateSut();

            var myWindow = (Window) sut.Create(new Uri("WpfLikeModel/WindowWithUserControl.xaml", UriKind.Relative));
            Assert.IsType(typeof (WindowWithUserControl), myWindow);
            Assert.Equal(myWindow.Title, "Hello World :)");
            Assert.IsType(typeof (UserControl), myWindow.Content);
            var userControl = (UserControl) myWindow.Content;
            Assert.Equal("It's-a me, Mario", userControl.Property);
            Assert.IsType(typeof (ChildClass), userControl.Content);
        }
    }
}