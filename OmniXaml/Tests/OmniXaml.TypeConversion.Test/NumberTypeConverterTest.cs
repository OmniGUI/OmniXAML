using System;

using OmniXaml.TypeConversion;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Perspex.Markup.Test
{
    using System.Globalization;

    [TestClass]
    public class NumberTypeConverterTest
    {
        [TestMethod]
        public void ConvertFromInt32ToString()
        {
            var c = new NumberTypeConverter();
            int value = 3;
            var converted = c.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(string));
            Assert.AreEqual("3", converted);
        }
    }
}
