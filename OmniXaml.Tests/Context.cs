namespace OmniXaml.Tests
{
    using Metadata;
    using Model;
    using ConstructionFragmentLoader = Model.ConstructionFragmentLoader;

    public static class Context
    {
        public static IMetadataProvider GetMetadataProvider()
        {
            var metadataProvider = new MetadataProvider();

            metadataProvider.Register(
                new GenericMetadata<Window>()
                .WithRuntimeNameProperty(window => window.Name)
                    .WithContentProperty(tb => tb.Content));

            metadataProvider.Register(
                new GenericMetadata<TextBlock>()
                    .WithContentProperty(tb => tb.Text));

            metadataProvider.Register(
                new GenericMetadata<ItemsControl>()
                    .WithContentProperty(tb => tb.Items));

            metadataProvider.Register(
                new GenericMetadata<DataTemplate>()
                    .WithFragmentLoader(tb => tb.Content, new ConstructionFragmentLoader()));

            return metadataProvider;
        }
    }
}