namespace OmniXaml.Wpf
{
    public static class WiringContextFactory
    {
        private static WiringContext context;

        public static WiringContext GetContext(ITypeFactory factory)
        {
            if (context == null)
            {
                var builder = new CleanWiringContextBuilder(factory);                  
                context = builder.Build();
            }

            return context;
        }
    }
}