namespace TestApplication
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using OmniXaml.Tests;
    using ViewModels;
    using Views;

    public class XamlVisualizerViewModel : ViewModel
    {
        public ICommand VisualizeTagsCommand { get; set; }
        public ICommand VisualizeTreeCommand { get; set; }
        public WiringContext WiringContext { get; protected set; }

        public XamlVisualizerViewModel()
        {
            VisualizeTagsCommand = new RelayCommand(xaml => OpenTagVisualizer(NodeVisualizer.ToTags(ConvertToNodes((string)xaml))));
            VisualizeTreeCommand = new RelayCommand(xaml => OpenTreeVisualizer(NodeVisualizer.ToTree(ConvertToNodes((string)xaml))));
        }

        private static void OpenTreeVisualizer(OmniXaml.Tests.Node root)
        {
            var visualizerWindow = new TreeVisualizerWindow { DataContext = new TreeVisualizerViewModel(root) };
            visualizerWindow.Show();
        }

        private void OpenTagVisualizer(IEnumerable<Tag> tags)
        {
            var visualizerWindow = new TagVisualizerWindow { DataContext = new TagVisualizerViewModel(tags) };
            visualizerWindow.Show();
        }

        private IEnumerable<XamlNode> ConvertToNodes(string xaml)
        {
            var wiringContext = WiringContext;
            var pullParser = new XamlNodesPullParser(wiringContext);
            var protoParser = new ProtoParser(wiringContext.TypeContext);
            return pullParser.Parse(protoParser.Parse(xaml));
        }
    }
}