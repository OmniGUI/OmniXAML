namespace Glass.ChangeTracking
{
    using System.Linq;

    public abstract class PropertyChain
    {
        public PropertyChain(string path)
        {            
            var propertyParts = path.Split('.');
            PropertyName = propertyParts.First();
            SubPath = string.Join(".", propertyParts.Skip(1));
        }

        public object Value => Child != null ? Child.Value : Property.Value;

        protected Property Property { get; set; }

        protected PropertyChain Child { get; set; }

        protected string PropertyName { get; }

        protected string SubPath { get; }
    }
}