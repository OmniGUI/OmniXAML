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
                {new AttributeBasedStringValueConverter(assemblies), new BuiltInConverter(),});
        }

        public object Load(string xaml, object intance = null)
        {
            var ct = Parse(xaml);
            return Construct(ct.Root, intance);
        }

        private object Construct(ConstructionNode ctNode, object instance)
        {
            var namescopeAnnotator = new NamescopeAnnotator(metadataProvider);
            var instanceCreator = GetInstanceCreator(converter);
            var objectConstructor = GetObjectBuilder(instanceCreator, converter);
            var construct = objectConstructor.Inflate(ctNode);
            return new ConstructionResult(construct, namescopeAnnotator);
        }

        protected virtual ISmartInstanceCreator GetInstanceCreator(IStringSourceValueConverter converter)
        {
            return new SmartInstanceCreator(converter);
        }

        protected virtual IObjectBuilder2 GetObjectBuilder(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter)
        {
            return new ObjectBuilder2(instanceCreator, converter);
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