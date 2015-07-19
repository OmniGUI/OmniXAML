using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApplication.Views
{
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
