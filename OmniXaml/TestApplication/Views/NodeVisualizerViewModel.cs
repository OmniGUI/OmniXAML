namespace TestApplication
{
    using System.Collections.Generic;
    using OmniXaml.Tests;

    public class NodeVisualizerViewModel : ViewModel
    {
        public NodeVisualizerViewModel(IEnumerable<Tag> tags)
        {
            Tags = tags;
        }

        public IEnumerable<Tag> Tags { get; set; }
    }
}