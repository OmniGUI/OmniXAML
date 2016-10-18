namespace Yuniversal.Adapters
{
    using OmniXaml;

    public class TemplateContent
    {
        private readonly ConstructionNode node;
        private readonly IObjectBuilder builder;

        public TemplateContent(ConstructionNode node, IObjectBuilder builder)
        {
            this.node = node;
            this.builder = builder;
        }

        public object Load()
        {
            return builder.Create(node);
        }

        protected bool Equals(TemplateContent other)
        {
            return Equals(node, other.node);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((TemplateContent) obj);
        }

        public override int GetHashCode()
        {
            return (node != null ? node.GetHashCode() : 0);
        }
    }
}