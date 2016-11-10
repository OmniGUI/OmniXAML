namespace OmniXaml.InlineParsers.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MarkupExtensionNodeToConstructionNodeConverter
    {
        private readonly ITypeDirectory typeDirectory;

        public MarkupExtensionNodeToConstructionNodeConverter(ITypeDirectory typeDirectory)
        {
            this.typeDirectory = typeDirectory;
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
            var type = typeDirectory.GetTypeByPrefix(identifier.Prefix, identifier.TypeName);
            
            if (type == null)
            {
                type = typeDirectory.GetTypeByPrefix(identifier.Prefix, identifier.TypeName + "Extension");
            }

            if (type == null)
            {
                throw new XamlParserException($"Cannot locate the type {identifier}:{identifier.TypeName}");
            }

            return type;
        }

        private IEnumerable<MameberAssignment> ParseAssignments(IEnumerable<PropertyOption> propertyOptions, Type parentType)
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