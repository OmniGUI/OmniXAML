namespace OmniXaml
{
    public class ExtendedObjectBuilder : ObjectBuilder
    {
        public ExtendedObjectBuilder(IInstanceCreator creator, ISourceValueConverter sourceValueConverter) : base(creator, sourceValueConverter)
        {
        }

        protected override void AssignFirstValueToNonCollection(object instance, object value, Property property)
        {
            var finalValue = ApplyValueConversionIfApplicable(value);

            if (instance.GetType().Name == "DataTemplate" && property.PropertyName == "Content")
            {
            }
            else
            {
                base.AssignFirstValueToNonCollection(instance, finalValue, property);
            }
        }

        private static object ApplyValueConversionIfApplicable(object value)
        {
            var me = value as IMarkupExtension;
            if (me != null)
            {
                return me.GetValue();
            }

            return value;
        }
    }
}