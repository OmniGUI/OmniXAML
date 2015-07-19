namespace OmniXaml
{
    using System.Collections.Generic;
    using System.IO;
    using Glass;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class ConfiguredXamlXmlLoader : IConfiguredXamlLoader
    {
        private readonly IObjectAssembler objectAssembler;
        private readonly SuperProtoParser protoParser;
        private readonly XamlNodesPullParser pullParser;

        public ConfiguredXamlXmlLoader(SuperProtoParser protoParser, XamlNodesPullParser pullParser, IObjectAssembler objectAssembler)  
        {
            Guard.ThrowIfNull(objectAssembler, nameof(objectAssembler));

            this.objectAssembler = objectAssembler;        
            this.protoParser = protoParser;
            this.pullParser = pullParser;
        }

        public object Load(Stream stream)
        {
            var xamlProtoNodes = protoParser.Parse(stream);
            var xamlNodes = pullParser.Parse(xamlProtoNodes);
            return Load(xamlNodes);
        }

        private object Load(IEnumerable<XamlNode> xamlNodes)
        {
            foreach (var xamlNode in xamlNodes)
            {
                objectAssembler.Process(xamlNode);
            }

            return objectAssembler.Result;
        }
    }
}