using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Perspex.Markup.Test
{
    [TestClass]
    public class ThicknessTypeConverterTest
    {
     

        //[TestMethod]
        //public void ConvertStringToThickness()
        //{
        //    var c = new ThicknessConverter();
        //    string value = "40, 30, 20, 10";
        //    var converted = c.ConvertFrom(null, CultureInfo.InvariantCulture, value, typeof(int));
        //    Assert.AreEqual(new Thickness(40, 30, 20, 10), converted);
        //}

        [TestMethod]
        public void ConvertStringToThickness()
        {
            var c = new ThicknessConverter();
            var value = "40, 30, 20, 10";
            var converted = c.ConvertFrom(null, CultureInfo.InvariantCulture, value);
            Assert.AreEqual(new Thickness(40, 30, 20, 10), converted);
        }
    }
}