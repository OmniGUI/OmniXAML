namespace OmniXaml.Parsers.ProtoParser.SuperProtoParser
{
    using System.Collections.ObjectModel;
    using System.Xml;
    using Glass;
    using Typing;

    internal class AttributeReader
    {
        private readonly XmlReader reader;

        public AttributeReader(XmlReader reader)
        {
            this.reader = reader;
        }

        public AttributeFeed Load()
        {
            var prefixDefinitions = new Collection<NsPrefix>();
            var attributes = new Collection<UnprocessedAttribute>();

            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    var longDescriptor = reader.Name;

                    if (longDescriptor.Contains("xmlns"))
                    {
                        prefixDefinitions.Add(GetPrefixDefinition());
                    }
                    else
                    {
                        attributes.Add(GetAttribute());
                    }

                } while (reader.MoveToNextAttribute());

                reader.MoveToElement();
            }

            return new AttributeFeed(prefixDefinitions, attributes);
        }

        private UnprocessedAttribute GetAttribute()
        {
            return new UnprocessedAttribute(PropertyLocator.Parse(reader.Name), reader.Value);
        }

        private NsPrefix GetPrefixDefinition()
        {
            var dicotomize = reader.Name.Dicotomize(':');
            return new NsPrefix(dicotomize.Item2 ?? string.Empty, reader.Value);
        }
    }
}