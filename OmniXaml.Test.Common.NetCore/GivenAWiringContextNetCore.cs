namespace OmniXaml.Tests.Common.NetCore
{
    using AppServices.NetCore;
    using Tests.Common;

    public class GivenAWiringContextNetCore : GivenAWiringContext
    {
        public GivenAWiringContextNetCore() : base(Assemblies.AssembliesInAppFolder)
        {
        }
    }
}