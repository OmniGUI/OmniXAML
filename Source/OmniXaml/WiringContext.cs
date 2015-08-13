namespace OmniXaml
{
    public class WiringContext : IWiringContext
    {
        public WiringContext(ITypeContext typeContext, ITypeFeatureProvider typeFeatureProvider)
        {
            FeatureProvider = typeFeatureProvider;
            TypeContext = typeContext;            
        }

        public ITypeContext TypeContext { get; }

        public ITypeFeatureProvider FeatureProvider { get; }
    }
}