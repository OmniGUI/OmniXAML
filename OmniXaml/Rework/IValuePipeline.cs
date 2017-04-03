namespace OmniXaml.Rework
{
    public interface IValuePipeline
    {
        void Handle(object parent, Member member, MutablePipelineUnit mutable);
    }
}