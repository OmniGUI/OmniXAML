namespace OmniXaml.Wpf
{
    public static class WpfWiringContextFactory
    {
        private static IWiringContext context;

        public static IWiringContext GetContext(ITypeFactory factory)
        {
            if (context == null)
            {                
                context = new WpfWiringContext(factory);
            }

            return context;
        }
    }
}