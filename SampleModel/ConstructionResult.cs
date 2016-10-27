namespace SampleModel
{
    using OmniXaml;

    internal class ConstructionResult
    {
        public object Instance { get; set; }
        public INamescopeAnnotator NamescopeAnnotator { get; set; }

        public ConstructionResult(object instance, INamescopeAnnotator namescopeAnnotator)
        {
            Instance = instance;
            NamescopeAnnotator = namescopeAnnotator;
        }
    }
}