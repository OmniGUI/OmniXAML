namespace OmniXaml
{
    public interface IObjectBuilder
    {
        object Create(ConstructionNode node);
    }

    public class TemplateAwareObjectBuilder : ObjectBuilder
    {
        private readonly IObjectBuilder inner;

        public TemplateAwareObjectBuilder(IInstanceCreator creator, ISourceValueConverter sourceValueConverter) : base(creator, sourceValueConverter)
        {
        }

        protected override void AssignFirstValueToNonCollection(object instance, object value, Property property)
        {
            if (instance.GetType().Name == "DataTemplate" && property.PropertyName == "Content")
            {
            }
            else
            {
                base.AssignFirstValueToNonCollection(instance, value, property);
            }
        }
    }
}