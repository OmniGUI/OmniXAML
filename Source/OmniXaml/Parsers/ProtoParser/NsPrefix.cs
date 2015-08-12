namespace OmniXaml.Parsers.ProtoParser
{
    internal class NsPrefix
    {
        public NsPrefix(string prefix, string ns)
        {
            Prefix = prefix;
            Namespace = ns;
        }

        public string Prefix { get; private set; }
        public string Namespace { get; private set; }
    }
}