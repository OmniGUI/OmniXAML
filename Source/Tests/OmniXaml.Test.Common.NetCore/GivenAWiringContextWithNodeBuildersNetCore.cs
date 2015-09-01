namespace OmniXaml.Tests.Common.NetCore
{
    using Services.DotNetFx;
    using Tests.Common;

    public class GivenAWiringContextWithNodeBuildersNetCore : GivenAWiringContextWithNodeBuilders
    {
        public GivenAWiringContextWithNodeBuildersNetCore() : base(Assemblies.AssembliesInAppFolder)
        {            
        }
    }
}