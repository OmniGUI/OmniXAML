using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OmniXaml.Tests
{
    using System.IO;
    using System.Reflection;

    [TestClass]
    public class XamlTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));

            var sut = new XamlToTreeParser(ass, new[] {"OmniXaml.Tests.Model"});
            var tree = sut.Parse("<Window Title=\"Saludos\" />");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));

            var sut = new XamlToTreeParser(ass, new[] {"OmniXaml.Tests.Model"});
            var tree = sut.Parse("<Window>" +
                                     "<Window.Content>Hola</Window.Content>" +
                                 "</Window>");
        }

        [TestMethod]
        public void TestMethod3()
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));

            var sut = new XamlToTreeParser(ass, new[] {"OmniXaml.Tests.Model"});
            var tree = sut.Parse("<Window>" +
                                     "<Window.Content><TextBlock /></Window.Content>" +
                                 "</Window>");
        }
    }  
}
