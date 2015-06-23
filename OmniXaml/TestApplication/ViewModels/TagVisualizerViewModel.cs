namespace TestApplication
{
    using System.Collections.Generic;
    using OmniXaml.Tests;

    public class TagVisualizerViewModel : ViewModel
    {
        public TagVisualizerViewModel(IEnumerable<VisualizationTag> tags)
        {
            Tags = tags;
        }

        public IEnumerable<VisualizationTag> Tags { get; set; }
    }
}