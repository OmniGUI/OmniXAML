namespace OmniXaml.Services
{
    using System.Collections.Generic;
    using System.Reflection;
    using Rework;
    using TypeLocation;

    public class XamlLoader : IXamlLoader
    {
        private readonly ITypeDirectory directory;
        private readonly AttributeBasedMetadataProvider metadataProvider;
        private readonly IStringSourceValueConverter converter;

        public XamlLoader(IList<Assembly> assemblies)
        {
            directory = new AttributeBasedTypeDirectory(assemblies);
            metadataProvider = new AttributeBasedMetadataProvider();
            converter = new SuperSmartSourceValueConverter(new IStringSourceValueConverter[]
            {
                new DirectCompatibilitySourceValueConverter(),
                new AttributeBasedStringValueConverter(assemblies), new TypeConverterSourceValueConverter(),
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
            var objectConstructor = GetObjectBuilder(instanceCreator, converter);
            var construct = objectConstructor.Inflate(ctNode);
            return construct;
        }

        protected virtual ISmartInstanceCreator GetInstanceCreator(IStringSourceValueConverter converter)
        {
            return new SmartInstanceCreator(converter);
        }

        protected virtual IObjectBuilder GetObjectBuilder(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter)
        {
            return new ObjectBuilder(instanceCreator, converter);
        }

        private ParseResult Parse(string xaml)
        {
            var resolver = new Resolver(directory);
            var sut = new XamlToTreeParser(metadataProvider, new[] {new InlineParser(resolver) }, resolver);

            var tree = sut.Parse(xaml, new PrefixAnnotator());
            return tree;
        }
    }
}