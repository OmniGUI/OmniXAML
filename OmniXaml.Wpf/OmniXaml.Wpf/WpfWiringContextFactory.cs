namespace OmniXaml.Wpf
{
    public static class WpfWiringContextFactory
    {
        private static WiringContext context;

        public static WiringContext GetContext(ITypeFactory factory)
        {
            if (context == null)
            {                
                context = new WpfWiringContext(factory);
            }

            return context;
        }
    }
}