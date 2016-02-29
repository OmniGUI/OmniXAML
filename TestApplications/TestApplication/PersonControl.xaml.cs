namespace SampleWpfApp
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for PersonControl.xaml
    /// </summary>
    public class PersonControl : UserControl
    {
        public PersonControl()
        {            
        }

        #region Person        
        public static readonly DependencyProperty PersonProperty =
          DependencyProperty.Register("Person", typeof(PersonViewModel), typeof(PersonControl),
            new FrameworkPropertyMetadata(null) { PropertyChangedCallback = PropertyChangedCallback} ) ;

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {           
        }

        public PersonViewModel Person
        {
            get { return (PersonViewModel)GetValue(PersonProperty); }
            set { SetValue(PersonProperty, value); }
        }

        #endregion


    }
}
