namespace OmniXaml.Tests
{
    using Classes;
    using Classes.Another;

    public class GivenAWiringContext
    {
        protected WiringContext WiringContext
        { get; }
        = new WiringContextBuilder()
            .AddNsForThisType("", "root", typeof(DummyClass))
            .AddNsForThisType("x", "another", typeof(Foreigner))
            .WithContentPropertiesFromAssemblies(new[] { typeof(DummyClass).Assembly })
            .Build();       
    }
}