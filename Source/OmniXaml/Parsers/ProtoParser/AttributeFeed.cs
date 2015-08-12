namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class AttributeFeed
    {
        private readonly Collection<NsPrefix> prefixDefinitions;
        private readonly Collection<UnprocessedAttribute> attributes;
        public Collection<RawDirective> Directives { get; }

        public AttributeFeed(Collection<NsPrefix> prefixDefinitions, Collection<UnprocessedAttribute> attributes, Collection<RawDirective> directives)
        {
            this.prefixDefinitions = prefixDefinitions;
            this.attributes = attributes;
            this.Directives = directives;
        }

        public IEnumerable<UnprocessedAttribute> RawAttributes => new ReadOnlyCollection<UnprocessedAttribute>(attributes);
        public IEnumerable<NsPrefix> PrefixRegistrations => new ReadOnlyCollection<NsPrefix>(prefixDefinitions);
    }
}