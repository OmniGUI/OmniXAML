namespace XamlViewer.ViewModels
{
    using System.Collections.Generic;
    using OmniXaml.Services.Mvvm;
    using OmniXaml.Visualization;

    public class TagVisualizerViewModel : ViewModel
    {
        public TagVisualizerViewModel(IEnumerable<VisualizationTag> tags)
        {
            Tags = tags;
        }

        public IEnumerable<VisualizationTag> Tags { get; set; }
    }
}