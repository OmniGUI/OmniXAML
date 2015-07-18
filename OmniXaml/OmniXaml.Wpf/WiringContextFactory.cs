namespace OmniXaml.Wpf
{
    public static class WiringContextFactory
    {
        public static WiringContext Create()
        {
            var builder = new CleanWiringContextBuilder();
            return builder.Build();
        }
    }
}