namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xaml.Tests.Resources;

    [TestClass]    
    public class NameScopeTests : GivenAXmlLoader
    {
        [TestMethod]
        public void RegisterOneChildInNameScope()
        {
            RuntimeTypeSource.ClearNamescopes();
            RuntimeTypeSource.EnableNameScope<DummyClass>();

            var actualInstance = Loader.FromString(Dummy.ChildInNameScope);
            var childInScope = ((DummyObject)actualInstance).Find("MyObject");
            Assert.IsInstanceOfType(childInScope, typeof(ChildClass));
        }

        [TestMethod]
        public void RegisterOneChildInNameScopeWithoutDirective()
        {
            RuntimeTypeSource.ClearNamescopes();
            RuntimeTypeSource.EnableNameScope<DummyClass>();

            var actualInstance = Loader.FromString(Dummy.ChildInNamescopeNoNameDirective);
            var childInScope = ((DummyObject)actualInstance).Find("MyObject");
            Assert.IsInstanceOfType(childInScope, typeof(ChildClass));
        }
    }
}