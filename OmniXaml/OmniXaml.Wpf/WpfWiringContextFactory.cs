namespace OmniXaml.Wpf
{
    public static class WpfWiringContextFactory
    {
        public static WiringContext Create()
        {
            var builder = new WpfCleanWiringContextBuilder();
            return builder.Build();
        }
    }
}