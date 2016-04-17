using System.Globalization;

using OmniXaml.TypeConversion;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Perspex.Markup.Test
{
    [TestClass]
    public class StringTypeConverterTest
    {
        [TestMethod]
        public void ConvertInt32ToString()
        {
            var c = new StringTypeConverter();
            var value = 3;
            var converted = c.ConvertFrom(null, CultureInfo.InvariantCulture, value);
            Assert.AreEqual("3", converted);
        }

        [TestMethod]
        public void ConvertStringToInt32()
        {
            var c = new StringTypeConverter();
            string value = "3";
            var converted = c.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(int));
            Assert.AreEqual(3, converted);
        }
    }
}