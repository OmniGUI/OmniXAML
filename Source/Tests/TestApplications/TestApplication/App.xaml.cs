namespace SampleWpfApp
{
    using System.Windows;
    using OmniXaml.Wpf;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewFactory = new WpfViewFactory();
            viewFactory.Show("Main");
        }
    }    
}