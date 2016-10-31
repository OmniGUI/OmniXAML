namespace OmniXaml
{
    using Metadata;

    public class ConstructionContext
    {
        public ConstructionContext(IInstanceCreator creator,
            ISourceValueConverter sourceValueConverter,
            IMetadataProvider metadataProvider)
        {
            Creator = creator;
            SourceValueConverter = sourceValueConverter;
            MetadataProvider = metadataProvider;
        }

        public IInstanceCreator Creator { get; }

        public ISourceValueConverter SourceValueConverter { get; }

        public IMetadataProvider MetadataProvider { get; }
    }
}