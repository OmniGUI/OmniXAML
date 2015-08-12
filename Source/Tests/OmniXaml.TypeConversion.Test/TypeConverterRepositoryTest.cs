using OmniXaml.TypeConversion;

namespace Perspex.Markup.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeConverterRepositoryTest
    {
        [TestMethod]
        public void GetConverterForInt32()
        {
            const int Number = 3;
            TypeConverterProvider typeConverterProvider = new TypeConverterProvider();
            var converter = typeConverterProvider.GetTypeConverter(Number.GetType());

            Assert.AreEqual(typeof(NumberTypeConverter), converter.GetType());
        }

        [TestMethod]
        public void GetConverterForInt64()
        {
            const long Number = 3;
            TypeConverterProvider typeConverterProvider = new TypeConverterProvider();
            var converter = typeConverterProvider.GetTypeConverter(Number.GetType());
            
            Assert.AreEqual(typeof(NumberTypeConverter), converter.GetType());
        }
    }
}
