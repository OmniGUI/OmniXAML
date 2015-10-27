namespace OmniXaml
{
    using System;
    using System.IO;
    using Parsers.ProtoParser;

    public class XamlXmlLoader : IXamlLoader
    {
        private readonly IXamlParserFactory xamlParserFactory;

        public XamlXmlLoader(IXamlParserFactory xamlParserFactory)
        {
            this.xamlParserFactory = xamlParserFactory;
        }

        public object Load(string str)
        {
            throw new NotImplementedException();
        }

        public object Load(Stream stream)
        {
            throw new NotImplementedException();
        }

        public object Load(IXmlReader reader)
        {
            return xamlParserFactory.CreateForReadingFree().Parse(reader);
        }

        public object Load(Stream stream, object instance)
        {
            throw new NotImplementedException();
        }

        public object Load(IXmlReader reader, object rootInstance)
        {
            return xamlParserFactory.CreateForReadingSpecificInstance(rootInstance).Parse(reader);
        }

        public object Load(string dummyclassXmlnsRootSamplepropertyValue, object rootInstance)
        {
            throw new NotImplementedException();
        }
    }
}