namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Builder;
    using Classes;
    using Classes.Another;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Typing;
    using Xaml.Tests.Resources;

    [TestClass]
    public class XamlXmlLoaderBuilderTests
    {
        [TestMethod]
        public void BasicConfigurationWithPrefixes()
        {
            var rootType = typeof(DummyClass);
            var anotherType = typeof(Foreigner);

            var sut = new XamlXmlLoaderBuilder();

            var definitionForRoot = Namespace
                .CreateMapFor(rootType.Namespace)
                .FromAssembly(rootType.Assembly)
                .To("root");

            var definitionForAnother = Namespace
                .CreateMapFor(anotherType.Namespace)
                .FromAssembly(anotherType.Assembly)
                .To("another");

            sut.WithNamespaces(new List<FullyConfiguredMapping> { definitionForRoot, definitionForAnother });
            sut.WithNsPrefixes(new List<PrefixRegistration>
            {
                new PrefixRegistration(string.Empty, "root"),
                new PrefixRegistration("x", "another")
            });

            var loader = sut.Build();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Dummy.DifferentNamespaces)))
            {
                var result = loader.Load(stream);
            }
        }
    }


}
