namespace OmniXaml.InlineParsers.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tests.Namespaces;
    using TypeLocation;

    public class MarkupExtensionNodeToConstructionNodeConverter
    {
        private const string ExtensionSuffix = "Extension";
        private readonly ITypeDirectory typeDirectory;
        private readonly Func<string, string> getNsFromPrefix;

        public MarkupExtensionNodeToConstructionNodeConverter(ITypeDirectory typeDirectory, Func<string, string> getNsFromPrefix)
        {
            this.typeDirectory = typeDirectory;
            this.getNsFromPrefix = getNsFromPrefix;
        }

        public ConstructionNode Convert(MarkupExtensionNode tree)
        {
            var identifier = tree.Identifier;
            var type = LocateType(identifier);

            var arguments = ParseArguments(tree.Options.OfType<PositionalOption>());
            var assignments = ParseAssignments(tree.Options.OfType<PropertyOption>(), type);

            return new ConstructionNode(type)
            {
                InjectableArguments = arguments,
                Assignments = assignments,
            };
        }

        private Type LocateType(IdentifierNode identifier)
        {
            var ns = getNsFromPrefix(identifier.Prefix);
            var type = typeDirectory.GetTypeByFullAddress(new Address(ns, identifier.TypeName));
            
            if (type == null)
            {
                type = typeDirectory.GetTypeByFullAddress(new Address(ns, identifier.TypeName + ExtensionSuffix));
            }

            if (type == null)
            {
                throw new XamlParserException($"Cannot locate the type {identifier.Prefix}:{identifier.TypeName}");
            }

            return type;
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
                        Children = new [] { Convert(markupExtensionNode) }
                    };
                });
        }

        private IEnumerable<string> ParseArguments(IEnumerable<PositionalOption> options)
        {
            return options.Select(option => option.Identifier);
        }
    }
}