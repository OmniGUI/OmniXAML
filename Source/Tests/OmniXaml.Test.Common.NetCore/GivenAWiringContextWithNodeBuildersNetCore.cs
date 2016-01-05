namespace OmniXaml.Tests.Common.NetCore
{
    using Classes;
    using Services.DotNetFx;
    using Tests.Common;

    public class GivenAWiringContextWithNodeBuildersNetCore : GivenAWiringContextWithNodeBuilders
    {
        public GivenAWiringContextWithNodeBuildersNetCore() : base(new[] { typeof(DummyClass).Assembly })
        {
        }
    }
}