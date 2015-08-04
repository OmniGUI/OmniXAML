namespace OmniXaml
{
    public class CleanWiringContextBuilder
    {
        public ITypeContext TypeContext { get; protected set; }
        public ITypeFeatureProvider TypeFeatureProvider { get; set; }

        public WiringContext Build()
        {            
            return new WiringContext(TypeContext, TypeFeatureProvider);
        }
    }
}