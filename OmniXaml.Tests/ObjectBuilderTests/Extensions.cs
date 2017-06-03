namespace OmniXaml.Tests.ObjectBuilderTests
{
    using Model.Custom;
    using Xunit;

    public class Extensions : ObjectBuilderTestsBase
    {

        [Fact]
        public void ParameterizedExtension()
        {
            var node = new ConstructionNode(typeof(ParametrizedExtension)) { PositionalParameter = new[] { "Hola" } };
            var extension = new ParametrizedExtension("Hola");
            var fixture = Create(node);

            Assert.Equal(extension, fixture.Result);
        }
    }
}