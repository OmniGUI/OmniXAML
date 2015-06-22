namespace TestApplication
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using OmniXaml.Tests;
    using OmniXaml.Tests.Classes;
    using Properties;

    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class XamlVisualizerViewModel : ViewModel
    {
        public ICommand VisualizeNodesCommand { get; set; }
        public WiringContext WiringContext { get; protected set; }

        public XamlVisualizerViewModel()
        {
            VisualizeNodesCommand = new RelayCommand(xaml => OpenVisualizer(NodeVisualizer.Convert(ConvertToNodes((string)xaml))));
        }

        private void OpenVisualizer(IEnumerable<Tag> tags)
        {
            var visualizerWindow = new Views.NodeVisualizerWindow();
            visualizerWindow.DataContext = new NodeVisualizerViewModel(tags);
            visualizerWindow.ShowDialog();
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