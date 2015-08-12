// ReSharper disable UnusedMember.Global
namespace XamlViewer.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using OmniXaml;

    /// <summary>
    /// Interaction logic for XamlTextBox.xaml
    /// </summary>
    public partial class XamlTextBox : UserControl
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

        #region WiringContext        
        public static readonly DependencyProperty WiringContextProperty =
          DependencyProperty.Register("WiringContext", typeof(WiringContext), typeof(XamlTextBox),
            new FrameworkPropertyMetadata(null));

        public WiringContext WiringContext
        {
            get { return (WiringContext)GetValue(WiringContextProperty); }
            set { SetValue(WiringContextProperty, value); }
        }

        #endregion
    }
}
