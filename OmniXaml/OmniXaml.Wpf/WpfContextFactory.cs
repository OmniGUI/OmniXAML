namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using Builder;
    using Typing;

    public static class WpfContextFactory
    {
        public static WiringContext Create()
        {
            var windowType = typeof(Window);
            var textBlockType = typeof(System.Windows.Controls.TextBlock);
            var toggleButtonType = typeof(ToggleButton);

            var rootNs = XamlNamespace
                .CreateMapFor(windowType.Namespace)
                .And(textBlockType.Namespace)
                .And(toggleButtonType.Namespace)
                .FromAssembly(windowType.Assembly)
                .To("root");

            var wiringContextBuilder = new WiringContextBuilder();

            wiringContextBuilder
                .WithNamespaces(new List<XamlNamespace> {rootNs})
                .WithNsPrefixes(new List<PrefixRegistration> {new PrefixRegistration("", "root")})
                .WithContentPropertyProvider(new WpfContentPropertyProvider())
                .WithConverterProvider(new WpfConverterProvider());

            return wiringContextBuilder.Build();
        }
    }
}