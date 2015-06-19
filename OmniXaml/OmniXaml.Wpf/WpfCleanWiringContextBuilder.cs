namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using Builder;
    using Typing;

    public class WpfCleanWiringContextBuilder : CleanWiringContextBuilder
    {
        private const string WpfRootNs = @"http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        public WpfCleanWiringContextBuilder()
        {
            var xamlNamespaceRegistry = new XamlNamespaceRegistry();

            var windowType = typeof(Window);
            var textBlockType = typeof(System.Windows.Controls.TextBlock);
            var toggleButtonType = typeof(ToggleButton);

            var bindingType = typeof(BindingExtension);

            var rootNs = XamlNamespace.Map(WpfRootNs)
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

            foreach (var ns in new List<XamlNamespace> { rootNs })
            {
                xamlNamespaceRegistry.AddNamespace(ns);
            }

            
            xamlNamespaceRegistry.RegisterPrefix(new PrefixRegistration("", WpfRootNs));            

            TypeContext = new TypeContext(new WpfXamlTypeRepository(xamlNamespaceRegistry), xamlNamespaceRegistry, new DefaultTypeFactory());
            ContentPropertyProvider = new WpfContentPropertyProvider();
            TypeConverterProvider = new WpfTypeConverterProvider();
        }
    }
}