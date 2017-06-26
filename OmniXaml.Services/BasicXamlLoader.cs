using System.Collections.Generic;
using System.Reflection;
using OmniXaml.Metadata;

namespace OmniXaml.Services
{
    public class BasicXamlLoader : XamlLoader
    {
        private INodeToObjectBuilder builder;

        public BasicXamlLoader(IList<Assembly> assemblies)
        {
            Assemblies = assemblies;
        }

        public IList<Assembly> Assemblies { get; }

        public override IXamlToTreeParser Parser => new XamlToTreeParser(MetadataProvider, InlineParsers, XmlTypeResolver);
        public override IMemberAssigmentApplier AssignmentApplier => new MemberAssigmentApplier(ValuePipeline);

        protected override IValuePipeline ValuePipeline => new NoActionValuePipeline();

        protected virtual IEnumerable<IInlineParser> InlineParsers => new List<IInlineParser>
        {
            new InlineParser(XmlTypeResolver)
        };

        protected virtual IXmlTypeResolver XmlTypeResolver => new XmlTypeXmlTypeResolver(new AttributeBasedTypeDirectory(Assemblies));

        protected virtual IMetadataProvider MetadataProvider => new AttributeBasedMetadataProvider();

        public override IInstanceCreator InstanceCreator => new SimpleInstanceCreator();

        public override IStringSourceValueConverter StringSourceValueConverter => new SuperSmartSourceValueConverter(
            new IStringSourceValueConverter[]
            {
                new DirectCompatibilitySourceValueConverter(),
                new AttributeBasedStringValueConverter(Assemblies), new ComponentModelTypeConverterBasedSourceValueConverter()
            });

        public override INodeToObjectBuilder Builder => builder ?? (builder = CreateBuilder());

        private INodeToObjectBuilder CreateBuilder()
        {
            return new NodeToObjectBuilder(new TwoPassesNodeAssembler(new NodeAssembler(InstanceCreator,
                StringSourceValueConverter, AssignmentApplier)));
        }
    }
}