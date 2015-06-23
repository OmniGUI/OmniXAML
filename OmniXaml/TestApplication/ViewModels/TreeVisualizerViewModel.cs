namespace TestApplication.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using OmniXaml.Tests;

    public class TreeVisualizerViewModel : ViewModel
    {
        public TreeVisualizerViewModel(Node root)
        {
            Nodes = new Collection<VisualizerNodeViewModel> { new VisualizerNodeViewModel(root) };
        }

        public IEnumerable<VisualizerNodeViewModel> Nodes { get; set; }
    }
}