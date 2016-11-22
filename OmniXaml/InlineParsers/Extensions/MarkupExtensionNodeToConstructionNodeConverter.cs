namespace OmniXaml.InlineParsers.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using TypeLocation;

    public class MarkupExtensionNodeToConstructionNodeConverter
    {
        private readonly Func<string, string> getNsFromPrefix;
        private readonly IResolver resolver;

        public MarkupExtensionNodeToConstructionNodeConverter(Func<string, string> getNsFromPrefix, IResolver resolver)
        {
            this.getNsFromPrefix = getNsFromPrefix;
            this.resolver = resolver;
        }

        public ConstructionNode Convert(MarkupExtensionNode tree)
        {
            var identifier = tree.Identifier;
            var ns = getNsFromPrefix(identifier.Prefix);
            var type = resolver.LocateMarkupExtension(XName.Get(identifier.TypeName, ns));

            var arguments = ParseArguments(tree.Options.OfType<PositionalOption>());
            var assignments = ParseAssignments(tree.Options.OfType<PropertyOption>(), type);

            return new ConstructionNode(type)
            {
                InjectableArguments = arguments,
                Assignments = assignments,
            };
        }
        
        private IEnumerable<MemberAssignment> ParseAssignments(IEnumerable<PropertyOption> propertyOptions, Type parentType)
        {
            return propertyOptions.Select(
                option =>
                {
                    var property = Member.FromStandard(parentType, option.Property);


                    var stringNode = option.Value as StringNode;
                    if (stringNode != null)
                    {
                        return new MemberAssignment
                        {
                            Member = property,
                            SourceValue = stringNode.Value,
                        };
                    }

                    var markupExtensionNode = option.Value as MarkupExtensionNode;
                    return new MemberAssignment
                    {
                        Member = property,
                        Children = new[] { Convert(markupExtensionNode) }
                    };
                });
        }

        private IEnumerable<string> ParseArguments(IEnumerable<PositionalOption> options)
        {
            return options.Select(option => option.Identifier);
        }
    }
}