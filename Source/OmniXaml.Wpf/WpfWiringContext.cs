namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using Builder;
    using Typing;

    public class WpfWiringContext : WiringContext
    {
        private const string WpfRootNs = @"http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        public WpfWiringContext(ITypeFactory factory) : base(GetTypeContext(factory), GetFeatureProvider())
        {           
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

        private static ITypeFeatureProvider GetFeatureProvider()
        {
            return new TypeFeatureProvider(new ContentPropertyProvider(), new TypeConverterProvider());
        }

        private static ITypeContext GetTypeContext(ITypeFactory typeFactory)
        {
            var xamlNamespaceRegistry = CreateXamlNamespaceRegistry();
            var xamlTypeRepository = new WpfXamlTypeRepository(xamlNamespaceRegistry, typeFactory, GetFeatureProvider());
            return new TypeContext(xamlTypeRepository, xamlNamespaceRegistry, typeFactory);
        }        
    }
}