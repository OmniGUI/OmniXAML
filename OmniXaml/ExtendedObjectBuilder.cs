namespace OmniXaml
{
    using System;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly Func<Assignment, StaticContext, TrackingContext, MarkupExtensionContext> createExtensionContext;

        public ExtendedObjectBuilder(StaticContext staticContext, Func<Assignment, StaticContext, TrackingContext, MarkupExtensionContext> createExtensionContext)
            : base(staticContext)
        {
            this.createExtensionContext = createExtensionContext;
        }

        protected override Assignment Transform(Assignment assignment, TrackingContext trackingContext)
        {
            var me = assignment.Value as IMarkupExtension;
            if (me != null)
            {
                var value = me.GetValue(createExtensionContext(assignment, StaticContext, trackingContext));
                assignment = assignment.ReplaceValue(value);
            }

            return base.Transform(assignment, trackingContext);
        }

        protected override object CreateForChild(object instance, Property property, ConstructionNode node, TrackingContext trackingContext)
        {
            var metadata = StaticContext.MetadataProvider.Get(instance.GetType());
            var fragmentLoaderInfo = metadata.FragmentLoaderInfo;

            if (fragmentLoaderInfo != null && instance.GetType() == fragmentLoaderInfo.Type && property.PropertyName == fragmentLoaderInfo.PropertyName)
            {
                return fragmentLoaderInfo.Loader.Load(node, this, trackingContext);
            }
            else
            {
                return base.CreateForChild(instance, property, node, trackingContext);
            }
        }       
    }
}