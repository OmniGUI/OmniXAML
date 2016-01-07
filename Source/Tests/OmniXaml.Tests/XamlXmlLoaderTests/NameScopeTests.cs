namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xaml.Tests.Resources;

    [TestClass]    
    public class NameScopeTests : GivenAXamlXmlLoader
    {
        [TestMethod]
        public void RegisterOneChildInNameScope()
        {
            TypeRuntimeTypeSource.ClearNamescopes();
            TypeRuntimeTypeSource.EnableNameScope<DummyClass>();

            var actualInstance = Loader.FromString(Dummy.ChildInNameScope);
            var childInScope = ((DummyObject)actualInstance).Find("MyObject");
            Assert.IsInstanceOfType(childInScope, typeof(ChildClass));
        }

        [TestMethod]
        public void RegisterOneChildInNameScopeWithoutDirective()
        {
            TypeRuntimeTypeSource.ClearNamescopes();
            TypeRuntimeTypeSource.EnableNameScope<DummyClass>();

            var actualInstance = Loader.FromString(Dummy.ChildInNamescopeNoNameDirective);
            var childInScope = ((DummyObject)actualInstance).Find("MyObject");
            Assert.IsInstanceOfType(childInScope, typeof(ChildClass));
        }
    }
}