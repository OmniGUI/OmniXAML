namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Xaml.Tests.Resources;
    using Xunit;

    public class MarkupExtensionsTests : GivenAXmlLoader
    {
        [Fact]
        public void SimpleExtension()
        {
            var actualInstance = Loader.FromString(File.LoadAsString(@"Xaml\Dummy\SimpleExtension.xaml"));

            Assert.IsType(typeof(DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal("Text From Markup Extension", dummyClass.SampleProperty);
        }

        [Fact]
        public void SimpleExtensionWithPropertyAssignment()
        {
            var actualInstance = Loader.FromString(File.LoadAsString(@"Xaml\Dummy\SimpleExtensionWithOneAssignment.xaml"));

            Assert.IsType(typeof(DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal("SomeValue", dummyClass.SampleProperty);
        }

        [Fact]
        public void ExtensionThatRetrievesInteger()
        {
            var actualInstance = Loader.FromString("<DummyClass xmlns=\"root\" Number=\"{Int Number=123}\"/>");

            Assert.IsType(typeof(DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal(123, dummyClass.Number);
        }

        [Fact]
        public void QuotedValue()
        {
            var actualInstance = Loader.FromString("<DummyClass xmlns=\"root\" SampleProperty=\"{Dummy Property=\'Some Value\'}\"/>");

            Assert.IsType(typeof(DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal("Some Value", dummyClass.SampleProperty);
        }

        [Fact]
        public void TwoQuotedValues()
        {
            var actualInstance = Loader.FromString(File.LoadAsString(@"Xaml\Dummy\MarkupExtensionTwoQuotedValues.xaml"));

            Assert.IsType(typeof(DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal("Some Value", dummyClass.SampleProperty);
            Assert.Equal("Another Value", dummyClass.AnotherProperty);
        }
    }
}
