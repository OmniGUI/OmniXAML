using System.Collections.Generic;
using System.Reflection;
using OmniXaml.Metadata;
using OmniXaml.Rework;
using OmniXaml.ReworkPhases;

namespace OmniXaml.Services
{
    public class XamlLoaderSimple : XamlLoaderBase
    {
        public XamlLoaderSimple(IList<Assembly> assemblies)
        {
            Assemblies = assemblies;
        }

        public IList<Assembly> Assemblies { get; }

        public override IXamlToTreeParser Parser => new XamlToTreeParser(MetadataProvider, InlineParsers, Resolver);
        public override IMemberAssigmentApplier AssignmentApplier => new MemberAssigmentApplier(ValuePipeline);

        protected override IValuePipeline ValuePipeline => new NoActionValuePipeline();

        protected virtual IEnumerable<IInlineParser> InlineParsers => new List<IInlineParser>
        {
            new InlineParser(Resolver)
        };

        protected virtual IResolver Resolver => new Resolver(new AttributeBasedTypeDirectory(Assemblies));

        protected virtual IMetadataProvider MetadataProvider => new AttributeBasedMetadataProvider();

        public override ISmartInstanceCreator SmartInstanceCreator =>
            new SmartInstanceCreator(StringSourceValueConverter);

        public override IStringSourceValueConverter StringSourceValueConverter => new SuperSmartSourceValueConverter(
            new IStringSourceValueConverter[]
            {
                new DirectCompatibilitySourceValueConverter(),
                new AttributeBasedStringValueConverter(Assemblies), new TypeConverterSourceValueConverter()
            });

        public override INodeToObjectBuilder Builder => new NodeToObjectBuilder(SmartInstanceCreator, StringSourceValueConverter, AssignmentApplier);
    }
}