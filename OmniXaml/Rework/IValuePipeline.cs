namespace OmniXaml.Rework
{
    public interface IValuePipeline
    {
        void Process(object parent, Member member, MutablePipelineUnit mutable);
    }
}