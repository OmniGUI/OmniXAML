namespace OmniXaml
{
    using Metadata;

    public class ConstructionContext
    {
        public ConstructionContext(IInstanceCreator creator,
            ISourceValueConverter sourceValueConverter,
            IMetadataProvider metadataProvider,
            IInstanceLifecycleSignaler signaler)
        {
            Creator = creator;
            SourceValueConverter = sourceValueConverter;
            Signaler = signaler;
            MetadataProvider = metadataProvider;
        }

        public IInstanceCreator Creator { get; }

        public ISourceValueConverter SourceValueConverter { get; }

        public IInstanceLifecycleSignaler Signaler { get; }

        public IMetadataProvider MetadataProvider { get; }
    }
}