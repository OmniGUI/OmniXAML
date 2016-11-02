namespace OmniXaml.Tests.Model
{
    using Metadata;

    public class TemplateContent
    {
        private readonly ConstructionNode node;
        private readonly IObjectBuilder builder;
        private readonly TrackingContext trackingContext;

        public TemplateContent(ConstructionNode node, IObjectBuilder builder, TrackingContext trackingContext)
        {
            this.node = node;
            this.builder = builder;
            this.trackingContext = trackingContext;
        }

        public object Load()
        {
            return builder.Create(node, trackingContext);
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