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
            WiringContext.EnableNameScope<DummyClass>();

            var actualInstance = XamlLoader.Load(Dummy.ChildInNameScope);
            var childInScope = ((DummyObject)actualInstance).Find("MyObject");
            Assert.IsInstanceOfType(childInScope, typeof(ChildClass));
        }

        [TestMethod]
        public void RegisterOneChildInNameScopeWithoutDirective()
        {
            WiringContext.EnableNameScope<DummyClass>();

            var actualInstance = XamlLoader.Load(Dummy.ChildInNamescopeNoNameDirective);
            var childInScope = ((DummyObject)actualInstance).Find("MyObject");
            Assert.IsInstanceOfType(childInScope, typeof(ChildClass));
        }
    }
}