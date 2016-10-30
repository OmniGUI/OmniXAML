namespace OmniXaml
{
    using Ambient;

    public class CreationContext
    {
        public CreationContext(INamescopeAnnotator annotator, IAmbientRegistrator ambientRegistrator)
        {
            this.Annotator = annotator;
            this.AmbientRegistrator = ambientRegistrator;
        }

        public INamescopeAnnotator Annotator { get; }

        public IAmbientRegistrator AmbientRegistrator { get; }
    }
}