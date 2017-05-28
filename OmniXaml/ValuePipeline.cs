namespace OmniXaml
{
    public abstract class ValuePipeline : IValuePipeline
    {
        private readonly IValuePipeline pipeline;

        protected ValuePipeline(IValuePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public void Handle(object parent, Member member, MutablePipelineUnit mutable, INodeToObjectBuilder builder, BuilderContext context)
        {
            if (!mutable.Handled)
            {
                pipeline.Handle(parent, member, mutable, builder, context);
                HandleCore(parent, member, mutable, builder, context);
            }
        }

        protected abstract void HandleCore(object parent, Member member, MutablePipelineUnit mutable, INodeToObjectBuilder builder, BuilderContext context);
    }
}