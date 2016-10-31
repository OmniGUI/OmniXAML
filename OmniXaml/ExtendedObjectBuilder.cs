namespace OmniXaml
{
    using System;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly Func<Assignment, ConstructionContext, MarkupExtensionContext> createExtensionContext;

        public ExtendedObjectBuilder(ConstructionContext constructionContext, Func<Assignment, ConstructionContext, MarkupExtensionContext> createExtensionContext)
            : base(constructionContext)
        {
            this.createExtensionContext = createExtensionContext;
        }

        protected override Assignment Transform(Assignment assignment)
        {
            var me = assignment.Value as IMarkupExtension;
            if (me != null)
            {
                var value = me.GetValue(createExtensionContext(assignment, ConstructionContext));
                assignment = assignment.ReplaceValue(value);
            }

            return base.Transform(assignment);
        }

        protected override object CreateForChild(object instance, Property property, ConstructionNode node, TrackingContext trackingContext)
        {
            var metadata = ConstructionContext.MetadataProvider.Get(instance.GetType());
            var fragmentLoaderInfo = metadata.FragmentLoaderInfo;

            if (fragmentLoaderInfo != null && instance.GetType() == fragmentLoaderInfo.Type && property.PropertyName == fragmentLoaderInfo.PropertyName)
            {
                return fragmentLoaderInfo.Loader.Load(node, this);
            }
            else
            {
                return base.CreateForChild(instance, property, node, trackingContext);
            }
        }       
    }
}