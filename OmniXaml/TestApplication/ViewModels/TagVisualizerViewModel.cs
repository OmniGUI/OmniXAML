namespace TestApplication
{
    using System.Collections.Generic;
    using OmniXaml.Tests;

    public class TagVisualizerViewModel : ViewModel
    {
        public TagVisualizerViewModel(IEnumerable<Tag> tags)
        {
            Tags = tags;
        }

        public IEnumerable<Tag> Tags { get; set; }
    }
}