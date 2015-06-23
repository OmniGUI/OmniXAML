namespace TestApplication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using OmniXaml.Tests;
    using OmniXaml.Visualization;
    using ViewModels;
    using Views;

    /// <summary>
    /// Interaction logic for XamlToolsControl.xaml
    /// </summary>
    public partial class XamlToolsControl
    {
        public XamlToolsControl()
        {
            InitializeComponent();

            VisualizeTagsCommand = new RelayCommand(Execute.Safely(_ => OpenTagVisualizer(NodeVisualizer.ToTags(ConvertToNodes(Xaml)))));
            VisualizeTreeCommand = new RelayCommand(Execute.Safely(_ => OpenTreeVisualizer(NodeVisualizer.ToTree(ConvertToNodes(Xaml)))));
        }

        #region Xaml        
        public static readonly DependencyProperty XamlProperty =
          DependencyProperty.Register("Xaml", typeof(string), typeof(XamlToolsControl),
            new FrameworkPropertyMetadata(null));

        #region WiringContext        
        public static readonly DependencyProperty WiringContextProperty =
          DependencyProperty.Register("WiringContext", typeof(WiringContext), typeof(XamlToolsControl),
            new FrameworkPropertyMetadata((WiringContext)null));

        #region IsShowAlwaysEnabled        
        public static readonly DependencyProperty IsShowAlwaysEnabledProperty =
          DependencyProperty.Register("IsShowAlwaysEnabled", typeof(bool), typeof(XamlToolsControl),
            new FrameworkPropertyMetadata(false));

        public bool IsShowAlwaysEnabled
        {
            get { return (bool)GetValue(IsShowAlwaysEnabledProperty); }
            set { SetValue(IsShowAlwaysEnabledProperty, value); }
        }

        #endregion

        public WiringContext WiringContext
        {
            get { return (WiringContext)GetValue(WiringContextProperty); }
            set { SetValue(WiringContextProperty, value); }
        }

        #endregion



        public string Xaml
        {
            get { return (string)GetValue(XamlProperty); }
            set { SetValue(XamlProperty, value); }
        }

        #endregion

        public ICommand VisualizeTagsCommand { get; set; }
        public ICommand VisualizeTreeCommand { get; set; }

        private static void OpenTreeVisualizer(VisualizationNode root)
        {
            var visualizerWindow = new TreeVisualizerWindow { DataContext = new TreeVisualizerViewModel(root) };
            visualizerWindow.Show();
        }

        private void OpenTagVisualizer(IEnumerable<VisualizationTag> tags)
        {
            var visualizerWindow = new TagVisualizerWindow { DataContext = new TagVisualizerViewModel(tags) };
            visualizerWindow.Show();
        }

        private IEnumerable<XamlNode> ConvertToNodes(string xaml)
        {
            var wiringContext = WiringContext;
            var pullParser = new XamlNodesPullParser(wiringContext);
            var protoParser = new ProtoParser(wiringContext.TypeContext);
            return pullParser.Parse(protoParser.Parse(xaml)).ToList();
        }
    }
}
