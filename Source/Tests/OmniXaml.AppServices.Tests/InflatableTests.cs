namespace OmniXaml.AppServices.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Tests.Classes.WpfLikeModel;

    [TestClass]
    public class InflatableTests : GivenAnInflatableTypeLoader
    {
        [TestMethod]
        public void InflatableInDataTemplateTest()
        {
            var actualInstance = this.Inflatable.Create<WindowWithTemplateAndUserControl>();
            Assert.IsInstanceOfType(actualInstance, typeof(WindowWithTemplateAndUserControl));
        }      
    }    
}
