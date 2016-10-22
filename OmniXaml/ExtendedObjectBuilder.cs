namespace OmniXaml
{
    using Metadata;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly IMetadataProvider metadataProvider;

        public ExtendedObjectBuilder(IInstanceCreator creator, ISourceValueConverter sourceValueConverter, IMetadataProvider metadataProvider, IInstanceLifecycleSignaler signaler)
            : base(creator, sourceValueConverter, signaler, metadataProvider)
        {
            this.metadataProvider = metadataProvider;
        }

        //protected override void OnPropertyAssignment(Assignment assignmentTarget)
        //{
        //    assignmentTarget = Transform(assignmentTarget);
        //    assignmentTarget.ExecuteAssignment();
        //}

        //protected override Assignment Transform(Assignment assignmentTarget)
        //{
        //    var me = assignmentTarget.Value as IMarkupExtension;
        //    if (me != null)
        //    {
        //        var value = me.GetValue(null);
        //        assignmentTarget = assignmentTarget.ChangeValue(value);
        //    }

        //    return assignmentTarget;
        //}

        //protected override object CreateForChild(object instance, Property property, ConstructionNode node)
        //{
        //    var metadata = metadataProvider.Get(instance.GetType());
        //    var fragmentLoaderInfo = metadata.FragmentLoaderInfo;

        //    if (fragmentLoaderInfo != null && instance.GetType() == fragmentLoaderInfo.Type && property.PropertyName == fragmentLoaderInfo.PropertyName)
        //    {
        //        return fragmentLoaderInfo.Loader.Load(node, this);
        //    }
        //    else
        //    {
        //        return base.CreateForChild(instance, property, node);
        //    }
        //}
    }
}