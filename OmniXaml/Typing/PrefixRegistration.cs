namespace OmniXaml.Typing
{
    public class PrefixRegistration
    {
        private readonly string prefix;

        private readonly string ns;

        public PrefixRegistration(string prefix, string ns)
        {
            this.prefix = prefix;
            this.ns = ns;
        }

        public string Prefix
        {
            get
            {
                return prefix;
            }
        }

        public string Ns
        {
            get
            {
                return ns;
            }
        }
    }
}