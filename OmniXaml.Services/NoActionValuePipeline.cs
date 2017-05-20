namespace OmniXaml.Services
{
    using Rework;

    public class NoActionValuePipeline : IValuePipeline
    {
        public void Handle(object parent, Member member, MutablePipelineUnit mutable, INodeToObjectBuilder builder)
        {            
        }
    }
}