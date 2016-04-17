namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Xunit;
    using Xaml.Tests.Resources;
  
    public class NameScopeTests : GivenAXmlLoader
    {
        [Fact]
        public void RegisterOneChildInNameScope()
        {
            RuntimeTypeSource.ClearNamescopes();
            RuntimeTypeSource.EnableNameScope<DummyClass>();

            var actualInstance = Loader.FromString(File.LoadAsString(@"Xaml\Dummy\ChildInNameScope.xaml"));
            var childInScope = ((DummyObject)actualInstance).Find("MyObject");
            Assert.IsType(typeof(ChildClass), childInScope);
        }

        [Fact]
        public void RegisterOneChildInNameScopeWithoutDirective()
        {
            RuntimeTypeSource.ClearNamescopes();
            RuntimeTypeSource.EnableNameScope<DummyClass>();

            var actualInstance = Loader.FromString(File.LoadAsString(@"Xaml\Dummy\ChildInNamescopeNoNameDirective.xaml"));
            var childInScope = ((DummyObject)actualInstance).Find("MyObject");
            Assert.IsType(typeof(ChildClass), childInScope);
        }
    }
}