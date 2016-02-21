namespace OmniXaml.Services.Tests
{
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using Xunit;

    public class InflatableTests : GivenAnInflatableTypeLoader
    {
        [Fact(Skip = "Ignore")]
        public void InflatableInDataTemplateTest()
        {
            var actualInstance = TypeFactory.Create<WindowWithTemplateAndUserControl>();
            Assert.IsType(typeof(WindowWithTemplateAndUserControl), actualInstance);
        }      
    }    
}
