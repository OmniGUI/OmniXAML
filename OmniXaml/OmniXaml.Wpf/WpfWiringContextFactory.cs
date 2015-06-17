namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using Builder;
    using TypeConversion;
    using Typing;

    public static class WpfWiringContextFactory
    {
        public static WiringContext Create()
        {
            var windowType = typeof(Window);
            var textBlockType = typeof(System.Windows.Controls.TextBlock);
            var toggleButtonType = typeof(ToggleButton);

            var bindingType = typeof(BindingExtension);

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

            var typeContextBuilder = new TypeContextBuilder()
                .WithNamespaces(new[] {rootNs})
                .WithNsPrefixes(new List<PrefixRegistration> {new PrefixRegistration("", "root")});

            var contentPropertyProvider = new WpfContentPropertyProvider();            
            ITypeConverterProvider typeConverterProvider = new WpfConverterProvider();

            return new WpfWiringContext(typeContextBuilder.Build(), contentPropertyProvider, typeConverterProvider);
        }
    }

    public class WpfWiringContext : WiringContext
    {
        public WpfWiringContext(ITypeContext typeContext, IContentPropertyProvider contentPropertyProvider, ITypeConverterProvider converterProvider)
            : base(typeContext, contentPropertyProvider, converterProvider)
        {
        }
    }
}