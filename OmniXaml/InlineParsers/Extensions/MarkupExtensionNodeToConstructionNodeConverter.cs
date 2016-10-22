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
            var type = typeDirectory.GetTypeByPrefix(identifier.Prefix, identifier.TypeName);

            var arguments = ParseArguments(tree.Options.OfType<PositionalOption>());
            var assignments = ParseAssignments(tree.Options.OfType<PropertyOption>(), type);

            return new ConstructionNode(type)
            {
                InjectableArguments = arguments,
                Assignments = assignments,
            };
        }

        private IEnumerable<PropertyAssignment> ParseAssignments(IEnumerable<PropertyOption> propertyOptions, Type parentType)
        {
            return propertyOptions.Select(
                option =>
                {
                    var property = Property.RegularProperty(parentType, option.Property);
                    

                    var stringNode = option.Value as StringNode;
                    if (stringNode != null)
                    {
                        return new PropertyAssignment
                        {
                            Property = property,
                            SourceValue = stringNode.Value,
                        };
                    }

                    var markupExtensionNode = option.Value as MarkupExtensionNode;
                    return new PropertyAssignment
                    {
                        Property = property,
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