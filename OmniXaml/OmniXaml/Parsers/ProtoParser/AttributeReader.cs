namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.ObjectModel;
    using Glass;
    using Typing;

    internal class AttributeReader
    {
        private readonly IXmlReader reader;

        public AttributeReader(IXmlReader reader)
        {
            this.reader = reader;
        }

        public AttributeFeed Load()
        {
            var prefixDefinitions = new Collection<NsPrefix>();
            var attributes = new Collection<UnprocessedAttribute>();
            var directives = new Collection<RawDirective>();

            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    var longDescriptor = reader.Name;

                    if (longDescriptor.Contains("xmlns"))
                    {
                        prefixDefinitions.Add(GetPrefixDefinition());
                    }
                    else if (longDescriptor.Contains("x:Key"))
                    {
                        directives.Add(GetDirective());
                    }
                    else
                    {
                        attributes.Add(GetAttribute());
                    }

                } while (reader.MoveToNextAttribute());

                reader.MoveToElement();
            }

            return new AttributeFeed(prefixDefinitions, attributes, directives);
        }

        private RawDirective GetDirective()
        {
            return new RawDirective(PropertyLocator.Parse(reader.Name), reader.Value);
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