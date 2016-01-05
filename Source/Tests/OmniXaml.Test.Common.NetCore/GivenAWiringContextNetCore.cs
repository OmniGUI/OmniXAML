namespace OmniXaml.Tests.Common.NetCore
{
    using Classes;
    using Services.DotNetFx;
    using Common;

    public class GivenAWiringContextNetCore : GivenAWiringContext
    {
        public GivenAWiringContextNetCore() : base(new[] { typeof(DummyClass).Assembly })
        {
        }
    }
}