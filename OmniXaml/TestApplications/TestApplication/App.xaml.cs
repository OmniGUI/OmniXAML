namespace RealTest
{
    using System.Windows;
    using OmniXaml.Wpf;
    using TestApplication;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var typeFactory = new WpfInflatableTypeFactory();
            var window = typeFactory.Create<MainWindow>();
            window.DataContext = new MainViewModel();
            window.ShowDialog();
        }
    }
}

