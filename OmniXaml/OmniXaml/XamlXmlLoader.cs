namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Glass;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class XamlXmlLoader : IXamlLoader
    {
        private readonly IObjectAssembler objectAssembler;
        private readonly WiringContext wiringContext;

        public XamlXmlLoader(IObjectAssembler objectAssembler, WiringContext wiringContext)            
        {
            Guard.ThrowIfNull(objectAssembler, nameof(objectAssembler));
            Guard.ThrowIfNull(wiringContext, nameof(wiringContext));

            this.objectAssembler = objectAssembler;
            this.wiringContext = wiringContext;
        }

        public object Load(Stream stream)
        {
            using (var t = new StreamReader(stream))
                return Load(t.ReadToEnd(), null);
        }

        public object Load(Stream stream, object rootInstance)
        {
            throw new NotImplementedException();
        }

        private object Load(string xml, object rootInstance)
        {
            var pullParser = new XamlNodesPullParser(wiringContext);
            var xamlNodes = pullParser.Parse(new ProtoParser(wiringContext.TypeContext).Parse(xml));
            return Load(xamlNodes, rootInstance);
        }

        private object Load(IEnumerable<XamlNode> xamlNodes, object rootInstance)
        {
            foreach (var xamlNode in xamlNodes)
            {
                objectAssembler.WriteNode(xamlNode);
            }

            return objectAssembler.Result;
        }
    }   
}