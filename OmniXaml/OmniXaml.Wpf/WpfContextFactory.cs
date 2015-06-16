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

            var bindingType = typeof (BindingExtension);

            var rootNs = XamlNamespace.Map("root")
                .With(
                    new[]
                    {
                        Route.Assembly(bindingType.Assembly).WithNamespaces(
                            new[] {bindingType.Namespace}),
                        Route.Assembly(windowType.Assembly).WithNamespaces(
                            new[]
                            {
                                windowType.Namespace,
                                textBlockType.Namespace,
                                toggleButtonType.Namespace,
                            })
                    });

            var wiringContextBuilder = new WiringContextBuilder();

            wiringContextBuilder
                .WithNamespaces(new List<XamlNamespace> { rootNs })
                .WithNsPrefixes(new List<PrefixRegistration> {new PrefixRegistration("", "root")})
                .WithContentPropertyProvider(new WpfContentPropertyProvider())
                .WithConverterProvider(new WpfConverterProvider());

            return wiringContextBuilder.Build();
        }
    }
}