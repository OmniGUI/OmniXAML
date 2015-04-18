namespace OmniXaml.Typing
{
    using System.Collections.Generic;

    public static class CoreTypes
    {
        private static readonly XamlDirective ItemsField = new XamlDirective("_Items", XamlType.Builder.CreateForBuiltInType(typeof(List<object>)));
        private static readonly XamlDirective InitializationField = new XamlDirective("_Initialization");
        private static readonly XamlDirective MarkupExtensionArgumentsField = new XamlDirective("_MarkupExtensionArguments");
        private static readonly XamlDirective UnknownContentField = new XamlDirective("_UnknownContent");

        public static XamlDirective Items => ItemsField;
        public static XamlDirective Initialization => InitializationField;
        public static XamlDirective UnknownContent => UnknownContentField;
        public static XamlType String => XamlType.Builder.CreateForBuiltInType(typeof(string));
        public static object MarkupExtensionArguments => MarkupExtensionArgumentsField;
    }
}