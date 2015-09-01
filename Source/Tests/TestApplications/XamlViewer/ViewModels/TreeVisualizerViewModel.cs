namespace XamlViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using OmniXaml.Services.Mvvm;
    using OmniXaml.Visualization;

    public class TreeVisualizerViewModel : ViewModel
    {
        public TreeVisualizerViewModel(VisualizationNode root)
        {
            Nodes = new Collection<VisualizerNodeViewModel> { new VisualizerNodeViewModel(root) };
        }

        public IEnumerable<VisualizerNodeViewModel> Nodes { get; set; }
    }
}