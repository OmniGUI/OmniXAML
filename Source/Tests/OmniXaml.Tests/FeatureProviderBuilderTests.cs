namespace OmniXaml.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FeatureProviderBuilderTests
    {
        [TestMethod]
        public void FromAttributes()
        {
            var builder = new TypeFeatureProviderBuilder();
            var collectionOfTypes = new Collection<Type> {typeof (TypeWithFeatures)};
            var provider = builder.FromAttributes(collectionOfTypes).Build();            
            Assert.IsTrue(provider.ContentProperties.Any());
            Assert.IsTrue(provider.TypeConverters.Any());
        }
    }
}