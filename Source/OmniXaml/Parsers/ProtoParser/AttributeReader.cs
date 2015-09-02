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
            var directives = new Collection<DirectiveAssignment>();

            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    var longDescriptor = reader.Name;

                    if (longDescriptor.Contains("xmlns"))
                    {
                        prefixDefinitions.Add(GetPrefixDefinition());
                    }
                    else if (IsDirective(longDescriptor))
                    {
                        directives.Add(GetDirective(longDescriptor));
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

        private bool IsDirective(string longDescriptor)
        {
            return longDescriptor.StartsWith("x:");
        }

        private DirectiveAssignment GetDirective(string longDescriptor)
        {
            var pair = longDescriptor.Dicotomize(':');
            return new DirectiveAssignment(CoreTypes.GetDirective(pair.Item2), reader.Value);
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