namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using Builder;
    using Typing;

    public class WpfRuntimeTypeSource : RuntimeTypeSource
    {
        private const string WpfRootNs = @"http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        
        public WpfRuntimeTypeSource()
            : base(new WpfTypeRepository(CreateXamlNamespaceRegistry(), new WpfXamlLoaderTypeFactory(), GetFeatureProvider()), CreateXamlNamespaceRegistry())
        {
        }

        private static NamespaceRegistry CreateXamlNamespaceRegistry()
        {
            var xamlNamespaceRegistry = new NamespaceRegistry();

            var windowType = typeof (Window);
            var textBlockType = typeof (TextBlock);
            var toggleButtonType = typeof (ToggleButton);
            var rotateTransformType = typeof (RotateTransform);
            var bindingType = typeof (BindingExtension);

            var rootNs = XamlNamespace.Map(WpfRootNs)
                .With(
                    new[]
                    {
                        Route.Assembly(bindingType.Assembly).WithNamespaces(
                            new[] {bindingType.Namespace}),
                        Route.Assembly(rotateTransformType.Assembly).WithNamespaces(
                            new[] {rotateTransformType.Namespace}),
                        Route.Assembly(bindingType.Assembly).WithNamespaces(
                            new[] {bindingType.Namespace}),
                        Route.Assembly(windowType.Assembly).WithNamespaces(
                            new[]
                            {
                                windowType.Namespace,
                                textBlockType.Namespace,
                                toggleButtonType.Namespace
                            })
                    });

            foreach (var ns in new List<XamlNamespace> {rootNs})
            {
                xamlNamespaceRegistry.AddNamespace(ns);
            }

            xamlNamespaceRegistry.RegisterPrefix(new PrefixRegistration("", WpfRootNs));

            return xamlNamespaceRegistry;
        }

        private static ITypeFeatureProvider GetFeatureProvider()
        {
            return new WpfTypeFeatureProvider(new TypeConverterProvider());
        }
    }
}