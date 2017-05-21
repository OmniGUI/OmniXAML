namespace OmniXaml
{
    public interface IValuePipeline
    {
        void Handle(object parent, Member member, MutablePipelineUnit mutable, INodeToObjectBuilder builder);
    }
}