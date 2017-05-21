namespace OmniXaml.Services
{
    public class NoActionValuePipeline : IValuePipeline
    {
        public void Handle(object parent, Member member, MutablePipelineUnit mutable, INodeToObjectBuilder builder)
        {            
        }
    }
}