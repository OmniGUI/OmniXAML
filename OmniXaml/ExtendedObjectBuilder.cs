namespace OmniXaml
{
    using Metadata;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly IMetadataProvider metadataProvider;

        public ExtendedObjectBuilder(IInstanceCreator creator, ISourceValueConverter sourceValueConverter, IMetadataProvider metadataProvider, IInstanceLifecycleSignaler signaler)
            : base(creator, sourceValueConverter, signaler)
        {
            this.metadataProvider = metadataProvider;
        }

        protected override void OnPropertyAssignment(AssignmentTarget assignmentTarget)
        {
            assignmentTarget = Transform(assignmentTarget);
            assignmentTarget.ExecuteAssignment();
        }

        protected override AssignmentTarget Transform(AssignmentTarget assignmentTarget)
        {
            var me = assignmentTarget.Value as IMarkupExtension;
            if (me != null)
            {
                var value = me.GetValue(null);
                assignmentTarget = assignmentTarget.ChangeValue(value);
            }

            return assignmentTarget;
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
    }
}