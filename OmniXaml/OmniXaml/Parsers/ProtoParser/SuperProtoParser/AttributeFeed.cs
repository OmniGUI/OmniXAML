namespace OmniXaml.Parsers.ProtoParser.SuperProtoParser
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class AttributeFeed
    {
        private readonly Collection<NsPrefix> prefixDefinitions;
        private readonly Collection<UnprocessedAttribute> attributes;

        public AttributeFeed(Collection<NsPrefix> prefixDefinitions, Collection<UnprocessedAttribute> attributes)
        {
            this.prefixDefinitions = prefixDefinitions;
            this.attributes = attributes;
        }

        public IEnumerable<UnprocessedAttribute> RawAttributes => new ReadOnlyCollection<UnprocessedAttribute>(attributes);
        public IEnumerable<NsPrefix> PrefixRegistrations => new ReadOnlyCollection<NsPrefix>(prefixDefinitions);
    }
}