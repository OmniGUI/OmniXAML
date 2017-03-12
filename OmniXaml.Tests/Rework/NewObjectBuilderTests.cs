namespace OmniXaml.Tests.Rework
{
    using Model;
    using Xunit;

    public class NewObjectBuilderTests
    {
        [Fact]
        public void SingleInstance()
        {
            var fixture = new ObjectBuildFixture();
            var instance = fixture.ObjectBuilder.Inflate(new ConstructionNode(typeof(Window)));
            
            Assert.IsType<Window>(instance);
        }
    }
}