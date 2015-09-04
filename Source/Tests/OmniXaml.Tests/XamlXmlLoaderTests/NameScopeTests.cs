namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xaml.Tests.Resources;

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
    }
}