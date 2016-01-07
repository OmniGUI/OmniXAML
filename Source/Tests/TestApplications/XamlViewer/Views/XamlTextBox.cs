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

        #region RuntimeTypeContext        
        public static readonly DependencyProperty RuntimeTypeContextProperty =
          DependencyProperty.Register("RuntimeTypeContext", typeof(IRuntimeTypeSource), typeof(XamlTextBox),
            new FrameworkPropertyMetadata(null));

        public IRuntimeTypeSource RuntimeTypeContext
        {
            get { return (IRuntimeTypeSource)GetValue(RuntimeTypeContextProperty); }
            set { SetValue(RuntimeTypeContextProperty, value); }
        }

        #endregion
    }
}
