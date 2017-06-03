namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class DirectiveExtractor
    {
        private const string SpecialNamespace = "special";

        public IEnumerable<Directive> GetDirectives(XElement node)
        {
            return from attr in node.Attributes()
                where !attr.IsNamespaceDeclaration && attr.Name.Namespace == SpecialNamespace
                select new Directive(attr.Name.LocalName, attr.Value);

        }
    }
}