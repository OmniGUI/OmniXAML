namespace OmniXaml.Tests
{
    using Classes;

    public class GivenAWiringContext
    {
        protected WiringContext WiringContext => DummyWiringContext.Create();
    }
}