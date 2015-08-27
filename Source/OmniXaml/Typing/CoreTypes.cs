namespace OmniXaml.Typing
{
    using System.Collections.Generic;

    public static class CoreTypes
    {
        private static readonly XamlDirective ItemsField = new XamlDirective("_Items", XamlType.CreateForBuiltInType(typeof(List<object>)));
        private static readonly XamlDirective InitializationField = new XamlDirective("_Initialization");
        private static readonly XamlDirective MarkupExtensionArgumentsField = new XamlDirective("_MarkupExtensionParameters");
        public static readonly XamlDirective Key = new XamlDirective("Key");
        public static readonly XamlDirective Name = new XamlDirective("Name");

        public static XamlDirective Items => ItemsField;
        public static XamlDirective Initialization => InitializationField;
        public static XamlType String => XamlType.CreateForBuiltInType(typeof(string));
        public static XamlType Int32 => XamlType.CreateForBuiltInType(typeof(int));
        public static object MarkupExtensionArguments => MarkupExtensionArgumentsField;        
    }
}