namespace OmniXaml
{
    public interface IWiringContext
    {
        ITypeContext TypeContext { get; }
        ITypeFeatureProvider FeatureProvider { get; }
    }
}