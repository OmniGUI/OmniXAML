namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using Builder;
    using Typing;

    public class CleanWiringContextBuilder : OmniXaml.CleanWiringContextBuilder
    {
        private const string WpfRootNs = @"http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        public CleanWiringContextBuilder(ITypeFactory factory)
        {
            var xamlNamespaceRegistry = CreateXamlNamespaceRegistry();
            var typeFactory = factory;
            TypeFeatureProvider = new TypeFeatureProvider(new ContentPropertyProvider(), new TypeConverterProvider());
            TypeContext = new TypeContext(new XamlTypeRepository(xamlNamespaceRegistry, typeFactory, TypeFeatureProvider), xamlNamespaceRegistry, typeFactory);
        }

        private static XamlNamespaceRegistry CreateXamlNamespaceRegistry()
        {
            var xamlNamespaceRegistry = new XamlNamespaceRegistry();

            var windowType = typeof(Window);
            var textBlockType = typeof(System.Windows.Controls.TextBlock);
            var toggleButtonType = typeof(ToggleButton);
            var rotateTransformType = typeof(RotateTransform);
            var bindingType = typeof(BindingExtension);

            var rootNs = XamlNamespace.Map(WpfRootNs)
                .With(
                    new[]
                    {
                        Route.Assembly(bindingType.Assembly).WithNamespaces(
                            new[] {bindingType.Namespace}),
                        Route.Assembly(rotateTransformType.Assembly).WithNamespaces(
                            new[] { rotateTransformType.Namespace}),
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

            return xamlNamespaceRegistry;
        }
    }
}