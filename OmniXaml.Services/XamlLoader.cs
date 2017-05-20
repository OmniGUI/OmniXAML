using System.Collections.Generic;
using System.Reflection;
using OmniXaml.Rework;
using OmniXaml.ReworkPhases;
using OmniXaml.TypeLocation;

namespace OmniXaml.Services
{
    public class XamlLoader : IXamlLoader
    {
        private readonly IStringSourceValueConverter converter;
        private readonly ITypeDirectory directory;
        private readonly AttributeBasedMetadataProvider metadataProvider;

        public XamlLoader(IList<Assembly> assemblies)
        {
            directory = new AttributeBasedTypeDirectory(assemblies);
            metadataProvider = new AttributeBasedMetadataProvider();
            converter = new SuperSmartSourceValueConverter(new IStringSourceValueConverter[]
            {
                new DirectCompatibilitySourceValueConverter(),
                new AttributeBasedStringValueConverter(assemblies), new TypeConverterSourceValueConverter()
            });
        }

        public object Load(string xaml, object intance = null)
        {
            var ct = Parse(xaml);
            return Construct(ct.Root, intance);
        }

        private object Construct(ConstructionNode ctNode, object instance)
        {
            var instanceCreator = GetInstanceCreator(converter);
            var objectConstructor = GetNodeToObjectBuilder(instanceCreator, converter,
                GetMemberAssignmentApplier(converter));
            var construct = objectConstructor.Build(ctNode);
            return construct;
        }

        protected virtual IMemberAssigmentApplier GetMemberAssignmentApplier(IStringSourceValueConverter converter)
        {
            return new MemberAssigmentApplier(GetValuePipeline(metadataProvider));
        }

        protected virtual IValuePipeline GetValuePipeline(AttributeBasedMetadataProvider metadataProvider)
        {
            return new NoActionValuePipeline();
        }

        protected virtual ISmartInstanceCreator GetInstanceCreator(IStringSourceValueConverter converter)
        {
            return new SmartInstanceCreator(converter);
        }

        public virtual INodeToObjectBuilder GetNodeToObjectBuilder(ISmartInstanceCreator instanceCreator,
            IStringSourceValueConverter converter, IMemberAssigmentApplier memberAssigmentApplier)
        {
            return new NodeToObjectBuilder(instanceCreator, converter, memberAssigmentApplier);
        }

        private ParseResult Parse(string xaml)
        {
            var resolver = new Resolver(directory);
            var sut = new XamlToTreeParser(metadataProvider, new[] {new InlineParser(resolver)}, resolver);

            var tree = sut.Parse(xaml, new PrefixAnnotator());
            return tree;
        }
    }
}