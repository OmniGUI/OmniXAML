namespace SampleModel
{
    using Model;
    using OmniXaml.DefaultLoader;
    using OmniXaml.Metadata;

    public static class Context
    {
        public static IMetadataProvider GetMetadataProvider()
        {
            var metadataProvider = new AttributeBasedMetadataProvider();

            return metadataProvider;
        }
    }
}