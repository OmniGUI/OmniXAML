namespace TestApplication
{
    using System.Windows;
    using OmniXaml.AppServices.Mvvm;
    using OmniXaml.AppServices.NetCore;
    using OmniXaml.Wpf;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var typeFactory = new WpfInflatableTypeFactory();

            var viewFactory = new ViewFactory(typeFactory);
            viewFactory.RegisterViews(ViewRegistration.FromTypes(Types.FromCurrentAddDomain));
            viewFactory.Show("Main");
        }     
    }    
}