namespace OmniXaml.Tests.Common.NetCore
{
    using Services.DotNetFx;
    using Tests.Common;

    public class GivenAWiringContextNetCore : GivenAWiringContext
    {
        public GivenAWiringContextNetCore() : base(Assemblies.AssembliesInAppFolder)
        {
        }
    }
}