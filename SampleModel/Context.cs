namespace SampleModel
{
    using Model;
    using OmniXaml.Metadata;

    public static class Context
    {
        public static IMetadataProvider GetMetadataProvider()
        {
            var metadataProvider = new MetadataProvider();

            metadataProvider.Register(
                new GenericMetadata<Zoo>()
                    .WithContentProperty(tb => tb.Animals));
                      
            return metadataProvider;
        }
    }
}