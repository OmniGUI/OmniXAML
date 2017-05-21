namespace OmniXaml
{
    public abstract class ValuePipeline : IValuePipeline
    {
        private readonly IValuePipeline pipeline;

        protected ValuePipeline(IValuePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public void Handle(object parent, Member member, MutablePipelineUnit mutable, INodeToObjectBuilder builder)
        {
            if (!mutable.Handled)
            {
                pipeline.Handle(parent, member, mutable, builder);
                HandleCore(parent, member, mutable, builder);
            }
        }

        protected abstract void HandleCore(object parent, Member member, MutablePipelineUnit mutable, INodeToObjectBuilder builder);
    }
}