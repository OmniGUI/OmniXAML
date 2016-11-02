namespace OmniXaml
{
    using System;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly Func<Assignment, ObjectBuilderContext, TrackingContext, ValueContext> createValueContext;

        public ExtendedObjectBuilder(ObjectBuilderContext objectBuilderContext, Func<Assignment, ObjectBuilderContext, TrackingContext, ValueContext> createValueContext)
            : base(objectBuilderContext, createValueContext)
        {
            this.createValueContext = createValueContext;
        }

        protected override Assignment ToCompatibleValue(Assignment assignment, TrackingContext trackingContext)
        {
            var me = assignment.Value as IMarkupExtension;
            if (me != null)
            {
                var value = me.GetValue(createValueContext(assignment, ObjectBuilderContext, trackingContext));
                assignment = assignment.ReplaceValue(value);
            }

            return base.ToCompatibleValue(assignment, trackingContext);
        }

        protected override object CreateForChild(object instance, Property property, ConstructionNode node, TrackingContext trackingContext)
        {
            var metadata = ObjectBuilderContext.MetadataProvider.Get(instance.GetType());
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