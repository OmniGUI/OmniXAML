namespace OmniXaml.Wpf
{
    using System;
    using System.Windows.Markup;
    using System.Xaml;

    internal class MarkupExtensionXamlType : XamlType, IProvideValueTarget
    {
        public MarkupExtensionXamlType(Type type, XamlSchemaContext schemaContext) : base(type, schemaContext)
        {
        }

        public object TargetObject { get; }

        public object TargetProperty { get; }
    }
}