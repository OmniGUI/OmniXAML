namespace OmniXaml.Typing
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class CoreTypes
    {
        public static readonly string SpecialNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

        private static readonly XamlDirective sItems = new XamlDirective("_Items", XamlType.CreateForBuiltInType(typeof(List<object>)));
        private static readonly XamlDirective sInitialization = new XamlDirective("_Initialization");
        private static readonly XamlDirective sMarkupExtensionParameters = new XamlDirective("_MarkupExtensionParameters");
        public static readonly XamlDirective sKey = new XamlDirective("Key");
        public static readonly XamlDirective sName = new XamlDirective("Name");

        public static XamlDirective Items => sItems;
        public static XamlDirective Initialization => sInitialization;
        public static XamlDirective MarkupExtensionArguments => sMarkupExtensionParameters;
        public static XamlDirective Name => sName;
        public static XamlDirective Key => sKey;

        public static XamlType String => XamlType.CreateForBuiltInType(typeof(string));
        public static XamlType Int32 => XamlType.CreateForBuiltInType(typeof(int));

        public static XamlDirective GetDirective(string name)
        {
            return Directives.First(directive => directive.Name == name);
        }

        public static IEnumerable<XamlDirective> Directives
        {
            get
            {
                var xamlDirectives = new[]
                {
                    Items,
                    Initialization,
                    MarkupExtensionArguments,
                    Key,
                    Name,
                };

                return new ReadOnlyCollection<XamlDirective>(xamlDirectives);
            }
        }
    }
}