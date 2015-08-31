namespace SampleWpfApp
{
    using System.Windows;
    using Grace.DependencyInjection;
    using OmniXaml.Wpf;

    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewFactory = new WpfViewFactory(new CustomGraceBasedTypeFactory(CreateDIContainer()));
            viewFactory.Show("Main");
        }

        private static IDependencyInjectionContainer CreateDIContainer()
        {
            var container = new DependencyInjectionContainer { ThrowExceptions = false };

            container.Configure(
                registration => registration
                    .Export<PeopleService>()
                    .As<IPeopleService>());

            return container;
        }
    }
}