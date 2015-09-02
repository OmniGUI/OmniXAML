namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Typing;

    internal class AttributeFeed
    {
        private readonly IEnumerable<NsPrefix> prefixDefinitions;
        private readonly string specialPrefix;
        private readonly IEnumerable<AttributeAssignment> attributes;
        private readonly IEnumerable<DirectiveAssignment> directives;

        public AttributeFeed(Collection<AttributeAssignment> assignments, string specialPrefix)
        {
            this.specialPrefix = specialPrefix;

            prefixDefinitions = assignments.Where(IsNamespaceDefinition).Select(GetNsDefinition);
            attributes = assignments.Where(IsAttribute);
            directives = assignments.Where(IsDirective).Select(ExtractDirectives);
        }

        private bool IsAttribute(AttributeAssignment assignment)
        {
            return !IsDirective(assignment) && !IsNamespaceDefinition(assignment);
        }

        private static NsPrefix GetNsDefinition(UnprocessedAttributeBase assignment)
        {
            if (assignment.Locator.Prefix == "xmlns")
            {
                return new NsPrefix(assignment.Locator.PropertyName, assignment.Value);
            }
            if (assignment.Locator.PropertyName == "xmlns")
            {
                return new NsPrefix("", assignment.Value);
            }

            throw new XamlParseException($"Cannot extract a Namespace Prefix Definition from this assignment: {assignment}");
        }

        private static bool IsNamespaceDefinition(AttributeAssignment assignment)
        {
            return assignment.Locator.IsNsPrefixDefinition;
        }

        private static DirectiveAssignment ExtractDirectives(AttributeAssignment assignment)
        {
            var xamlDirective = CoreTypes.GetDirective(assignment.Locator.PropertyName);
            return new DirectiveAssignment(xamlDirective, assignment.Value);
        }


        private bool IsDirective(AttributeAssignment attributeAssignment)
        {
            return attributeAssignment.Locator.Prefix == specialPrefix;
        }

        public IEnumerable<AttributeAssignment> RawAttributes => new ReadOnlyCollection<AttributeAssignment>(attributes.ToList());

        public IEnumerable<NsPrefix> PrefixRegistrations => new ReadOnlyCollection<NsPrefix>(prefixDefinitions.ToList());

        public IEnumerable<DirectiveAssignment> Directives => new ReadOnlyCollection<DirectiveAssignment>(directives.ToList());
    }
}