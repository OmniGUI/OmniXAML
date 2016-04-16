namespace XamlViewer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Glass.Core;
    using OmniXaml;
    using OmniXaml.Parsers.Parser;
    using OmniXaml.Parsers.ProtoParser;
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

            VisualizeTagsCommand = new RelayCommand(Execute.Safely(_ => OpenTagVisualizer(NodeVisualizer.ToTags(ConvertToNodes(Xaml.ToStream())))));
            VisualizeTreeCommand = new RelayCommand(Execute.Safely(_ => OpenTreeVisualizer(NodeVisualizer.ToTree(ConvertToNodes(Xaml.ToStream())))));
        }

        #region Xaml        
        public static readonly DependencyProperty XamlProperty =
          DependencyProperty.Register("Xaml", typeof(string), typeof(XamlToolsControl),
            new FrameworkPropertyMetadata(null));

        #region RuntimeTypeSource        
        public static readonly DependencyProperty RuntimeTypeSourceProperty =
          DependencyProperty.Register("RuntimeTypeSource", typeof(IRuntimeTypeSource), typeof(XamlToolsControl),
            new FrameworkPropertyMetadata((IRuntimeTypeSource)null));

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

        public IRuntimeTypeSource RuntimeTypeSource
        {
            get { return (IRuntimeTypeSource)GetValue(RuntimeTypeSourceProperty); }
            set { SetValue(RuntimeTypeSourceProperty, value); }
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

        private IEnumerable<Instruction> ConvertToNodes(Stream stream)
        {
            var reader = new XmlCompatibilityReader(stream);
            var runtimeTypeSource = RuntimeTypeSource;
            var pullParser = new InstructionParser(runtimeTypeSource);
            var protoParser = new ProtoInstructionParser(runtimeTypeSource);
            return pullParser.Parse(protoParser.Parse(reader)).ToList();
        }
    }
}
