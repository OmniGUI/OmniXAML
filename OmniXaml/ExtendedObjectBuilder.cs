namespace OmniXaml
{
    using Metadata;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly IMetadataProvider metadataProvider;

        public ExtendedObjectBuilder(IInstanceCreator creator, ISourceValueConverter sourceValueConverter, IMetadataProvider metadataProvider)
            : base(creator, sourceValueConverter, metadataProvider)
        {
            this.metadataProvider = metadataProvider;
        }

        protected override void AssignFirstValueToNonCollection(object instance, object value, Property property)
        {
            var finalValue = ApplyValueConversionIfApplicable(value);
            base.AssignFirstValueToNonCollection(instance, finalValue, property);
        }

        protected override object GatedCreate(object instance, Property property, ConstructionNode node)
        {
            var metadata = metadataProvider.Get(instance.GetType());
            var fragmentLoaderInfo = metadata.FragmentLoaderInfo;

            if (fragmentLoaderInfo != null && instance.GetType() == fragmentLoaderInfo.Type && property.PropertyName == fragmentLoaderInfo.PropertyName)
            {
                return fragmentLoaderInfo.Loader.Load(node, this);
            }
            else
            {
                return base.GatedCreate(instance, property, node);
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