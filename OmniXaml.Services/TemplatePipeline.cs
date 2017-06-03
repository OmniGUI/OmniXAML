using OmniXaml.Metadata;

namespace OmniXaml.Services
{
    public class TemplatePipeline : ValuePipeline
    {
        private readonly IMetadataProvider metadataProvider;

        public TemplatePipeline(IValuePipeline pipeline, IMetadataProvider metadataProvider) : base(pipeline)
        {
            this.metadataProvider = metadataProvider;
        }

        protected override void HandleCore(object parent, Member member, MutablePipelineUnit mutable, INodeToObjectBuilder builder, BuilderContext context)
        {
            var deferringLoader = metadataProvider.Get(parent.GetType()).FragmentLoaderInfo;

            if (deferringLoader == null)
            {
                return;
            }

            if (IsApplicable(deferringLoader, parent, member))
            {
                mutable.Value = deferringLoader.Loader.Load(mutable.ParentNode, builder, context);
            }
        }

        private static bool IsApplicable(FragmentLoaderInfo loader, object parent, Member member)
        {
            return loader.Type == parent.GetType() && string.Equals(member.MemberName, loader.PropertyName);
        }
    }
}