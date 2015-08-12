namespace OmniXaml.Wpf
{
    using System;
    using System.Windows.Markup;
    using System.Xaml;

    internal class MarkupExtensionXamlType : XamlType, IProvideValueTarget
    {
        private readonly object targetObject;
        private readonly object targetProperty;

        public MarkupExtensionXamlType(Type type, XamlSchemaContext schemaContext) : base(type, schemaContext)
        {
        }

        public object TargetObject
        {
            get { return targetObject; }
        }

        public object TargetProperty
        {
            get { return targetProperty; }
        }
    }
}