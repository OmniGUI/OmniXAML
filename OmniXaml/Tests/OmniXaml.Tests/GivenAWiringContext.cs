namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using Builder;
    using Classes;
    using Classes.Another;
    using Typing;
    using WiringContext = WiringContext;

    public class GivenAWiringContext
    {
        protected WiringContext WiringContext => DummyWiringContext.Create();
    }
}