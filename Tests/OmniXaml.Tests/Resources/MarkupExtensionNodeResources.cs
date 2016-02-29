namespace OmniXaml.Tests.Resources
{
    using System.Collections.Generic;
    using OmniXaml.Parsers.MarkupExtensions;

    public static class MarkupExtensionNodeResources
    {
        public static MarkupExtensionNode ComposedExtension()
        {
            return new MarkupExtensionNode(
                new IdentifierNode("BindingExtension"),
                new OptionsCollection(
                    new List<Option>
                    {
                        new PositionalOption("Width"),
                        new PropertyOption(
                            "RelativeSource",
                            new MarkupExtensionNode(
                                new IdentifierNode("RelativeSourceExtension"),
                                new OptionsCollection
                                {
                                    new PositionalOption("FindAncestor"),
                                    new PropertyOption("AncestorLevel", new StringNode("1")),
                                    new PropertyOption(
                                        "AncestorType",
                                        new MarkupExtensionNode(
                                            new IdentifierNode("x", "TypeExtension"),
                                            new OptionsCollection
                                            {
                                                new PositionalOption("Grid")
                                            }))

                                }))
                    }));
        }

        public static MarkupExtensionNode ComposedExtensionTemplateBindingWithConverter()
        {
            return new MarkupExtensionNode(
                new IdentifierNode("TemplateBindingExtension"),
                new OptionsCollection(
                    new List<Option>
                    {
                        new PropertyOption("Path", new StringNode("IsFloatingWatermarkVisible")),
                        new PropertyOption(
                            "Converter",
                            new MarkupExtensionNode(
                                new IdentifierNode("", "TypeExtension"),
                                new OptionsCollection
                                {
                                    new PositionalOption("FooBar")
                                }))

                    }));
        }
    }
}