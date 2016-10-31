namespace WpfApplication1
{
    using System.IO;
    using System.Windows;
    using Context;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = (Window) new XamlLoader().Load(File.ReadAllText("Sample.xml")).Instance;

            window.Show();
            MainWindow = window;
        }
    }
}