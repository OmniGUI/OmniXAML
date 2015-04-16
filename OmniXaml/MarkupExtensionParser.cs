namespace OmniXaml
{
    using System.Linq;
    using Sprache;

    public static class MarkupExtensionParser
    {
        private const char Quote = '\'';
        private const char OpenCurly = '{';
        private const char CloseCurly = '}';
        private const char Comma = ',';

        private static readonly Parser<TreeNode> QuotedValue = from firstQuote in Parse.Char(Quote)
            from identifier in Parse.CharExcept(new[] {Quote, OpenCurly, CloseCurly}).Many()
            from secondQuote in Parse.Char(Quote)
            select new StringNode(new string(identifier.ToArray()));

        private static readonly Parser<string> Identifier =            
            from first in Parse.Letter.Once()
            from rest in Parse.LetterOrDigit.Many()            
            select new string(first.Concat(rest).ToArray());

        private static readonly Parser<TreeNode> DirectValue = from value in Parse.LetterOrDigit.Many()
            select new StringNode(new string(value.ToArray()));
        
        private static readonly Parser<TreeNode> StringValueNode = QuotedValue.Or(DirectValue);
      
        public static readonly Parser<AssignmentNode> Assignment = from attr in Identifier
            from eqSign in Parse.Char('=')
            from value in AssignmentSource
            select new AssignmentNode(attr, value);

        private static readonly Parser<Option> Positional = from identifier in Identifier
            select new PositionalOption(identifier);

        private static readonly Parser<Option> Attribute = from identifier in Assignment
            select new PropertyOption(identifier.Property, identifier.Value);

        public static readonly Parser<OptionsCollection> Options = from options in Attribute.Or(Positional).DelimitedBy(Parse.Char(Comma)).Token()
            select new OptionsCollection(options);

        private static readonly Parser<MarkupExtensionNode> SimpleMarkupExtension = from openCurly in Parse.Char(OpenCurly)
            from identifier in Identifier
            from closeCurly in Parse.Char(CloseCurly)
            select new MarkupExtensionNode(identifier);

        private static readonly Parser<MarkupExtensionNode> MarkupExtensionWithOptions = from openCurly in Parse.Char(OpenCurly)
            from identifier in Identifier
            from space in Parse.WhiteSpace.AtLeastOnce()
            from options in Options.Once()
            from closeCurly in Parse.Char(CloseCurly)
            select new MarkupExtensionNode(identifier, options.First());

        public static readonly Parser<MarkupExtensionNode> MarkupExtension = MarkupExtensionWithOptions.Or(SimpleMarkupExtension);

        private static readonly Parser<TreeNode> AssignmentSource = MarkupExtension.Or(StringValueNode);
    }
}