using OmniXaml;
using OmniXaml.Rework;

namespace XamlLoadTest
{
    public abstract class ValuePipeline : IValuePipeline
    {
        private readonly IValuePipeline pipeline;

        protected ValuePipeline(IValuePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public void Handle(object parent, Member member, MutablePipelineUnit mutable)
        {
            if (!mutable.Handled)
            {
                pipeline.Handle(parent, member, mutable);
                HandleCore(parent, member, mutable);
            }
        }

        protected abstract void HandleCore(object parent, Member member, MutablePipelineUnit mutable);
    }
}