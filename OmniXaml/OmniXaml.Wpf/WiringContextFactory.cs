namespace OmniXaml.Wpf
{
    public static class WiringContextFactory
    {
        private static WiringContext context;

        public static WiringContext Context
        {
            get
            {
                if (context == null)
                {
                    var builder = new CleanWiringContextBuilder();
                    context = builder.Build();
                }

                return context;
            }
        }
    }
}