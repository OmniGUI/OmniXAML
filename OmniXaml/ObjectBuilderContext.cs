namespace OmniXaml
{
    using Metadata;

    public class ObjectBuilderContext
    {
        public ObjectBuilderContext(ISourceValueConverter sourceValueConverter,
            IMetadataProvider metadataProvider)
        {
            SourceValueConverter = sourceValueConverter;
            MetadataProvider = metadataProvider;
        }
        
        public ISourceValueConverter SourceValueConverter { get; }

        public IMetadataProvider MetadataProvider { get; }
    }
}