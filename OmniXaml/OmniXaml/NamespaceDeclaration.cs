namespace OmniXaml
{
    public class NamespaceDeclaration
    {
        private string ns;
        private string prefix;

        public NamespaceDeclaration(string ns, string prefix)
        {
            this.ns = ns;
            this.prefix = prefix;
        }

        public string Namespace
        {
            get { return ns; }
            set { ns = value; }
        }

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }
    }
}