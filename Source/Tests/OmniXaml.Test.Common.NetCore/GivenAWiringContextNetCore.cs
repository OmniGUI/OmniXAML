namespace OmniXaml.Tests.Common.NetCore
{
    using Services.DotNetFx;
    using Common;

    public class GivenAWiringContextNetCore : GivenAWiringContext
    {
        public GivenAWiringContextNetCore() : base(Assemblies.AssembliesInAppFolder)
        {
        }
    }
}