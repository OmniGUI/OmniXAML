namespace XamlViewer.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using OmniXaml;

    public partial class XamlTextBox
    {
        public XamlTextBox()
        {
            InitializeComponent();
        }

        #region Xaml        
        public static readonly DependencyProperty XamlProperty =
          DependencyProperty.Register("Xaml", typeof(string), typeof(XamlTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Xaml
        {
            get { return (string)GetValue(XamlProperty); }
            set { SetValue(XamlProperty, value); }
        }

        #endregion

        #region RuntimeTypeSource        
        public static readonly DependencyProperty RuntimeTypeSourceProperty =
          DependencyProperty.Register("RuntimeTypeSource", typeof(IRuntimeTypeSource), typeof(XamlTextBox),
            new FrameworkPropertyMetadata(null));

        public IRuntimeTypeSource RuntimeTypeSource
        {
            get { return (IRuntimeTypeSource)GetValue(RuntimeTypeSourceProperty); }
            set { SetValue(RuntimeTypeSourceProperty, value); }
        }

        #endregion
    }
}
