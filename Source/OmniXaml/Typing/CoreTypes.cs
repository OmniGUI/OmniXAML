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

        private static readonly Directive sItems = new Directive("_Items", XamlType.CreateForBuiltInType(typeof(List<object>)));
        private static readonly Directive sInitialization = new Directive("_Initialization");
        private static readonly Directive sMarkupExtensionParameters = new Directive("_MarkupExtensionParameters");
        private static readonly Directive sUnknownContent = new Directive("_UnknownContent");
        public static readonly Directive sKey = new Directive("Key");
        public static readonly Directive sName = new Directive("Name");

        public static Directive Items => sItems;
        public static Directive UnknownContent => sUnknownContent;
        public static Directive Initialization => sInitialization;
        public static Directive MarkupExtensionArguments => sMarkupExtensionParameters;
        public static Directive Name => sName;
        public static Directive Key => sKey;

        public static XamlType String => XamlType.CreateForBuiltInType(typeof(string));
        public static XamlType Int32 => XamlType.CreateForBuiltInType(typeof(int));

        public static Directive GetDirective(string name)
        {
            return Directives.First(directive => directive.Name == name);
        }

        public static IEnumerable<Directive> Directives
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
                    UnknownContent,
                };

                return new ReadOnlyCollection<Directive>(xamlDirectives);
            }
        }

        
    }
}