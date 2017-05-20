using OmniXaml.Metadata;
using OmniXaml.Rework;

namespace OmniXaml.Services
{
    public class TemplatePipeline : ValuePipeline
    {
        private readonly AttributeBasedMetadataProvider metadataProvider;

        public TemplatePipeline(IValuePipeline pipeline, AttributeBasedMetadataProvider metadataProvider) : base(pipeline)
        {
            this.metadataProvider = metadataProvider;
        }

        protected override void HandleCore(object parent, Member member, MutablePipelineUnit mutable)
        {
            var deferringLoader = metadataProvider.Get(parent.GetType()).FragmentLoaderInfo;

            if (deferringLoader == null)
            {
                return;
            }

            if (IsApplicable(deferringLoader, parent, member))
            {
                mutable.Value = deferringLoader.Loader.Load(mutable.ParentNode);
            }
        }

        private static bool IsApplicable(FragmentLoaderInfo loader, object parent, Member member)
        {
            return loader.Type == parent.GetType() && string.Equals(member.MemberName, loader.PropertyName);
        }
    }
}