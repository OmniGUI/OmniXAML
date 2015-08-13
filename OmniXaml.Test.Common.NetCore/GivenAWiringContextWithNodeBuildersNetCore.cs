namespace OmniXaml.Tests.Common.NetCore
{
    using AppServices.NetCore;
    using Tests.Common;

    public class GivenAWiringContextWithNodeBuildersNetCore : GivenAWiringContextWithNodeBuilders
    {
        public GivenAWiringContextWithNodeBuildersNetCore() : base(Assemblies.AssembliesInAppFolder)
        {            
        }
    }
}