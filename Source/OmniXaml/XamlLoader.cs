namespace OmniXaml
{
    using System.IO;

    public class XamlLoader : IXamlLoader
    {
        private readonly IXamlParserFactory xamlParserFactory;

        public XamlLoader(IXamlParserFactory xamlParserFactory)
        {
            this.xamlParserFactory = xamlParserFactory;
        }

        public object Load(Stream stream)
        {
            return xamlParserFactory.CreateForReadingFree().Parse(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            return xamlParserFactory.CreateForReadingSpecificInstance(rootInstance).Parse(stream);
        }
    }
}