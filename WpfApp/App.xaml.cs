namespace WpfApplication1
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using Context;
    using OmniXaml;
    using OmniXaml.TypeLocation;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var objectBuilder = new ObjectBuilder(new InstanceCreator(), Registrator.GetSourceValueConverter());     
            var cons = GetConstructionNode();

            var window = (Window) objectBuilder.Create(cons);

            window.Show();
            MainWindow = window;
        }

        private ConstructionNode GetConstructionNode()
        {
            var type = typeof(Window);
            var ass = type.Assembly;

            ITypeDirectory directory = new TypeDirectory();

            directory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));

            var configuredAssemblyWithNamespaces = Route
                .Assembly(ass)
                .WithNamespaces("System.Windows", "System.Windows.Controls");
            var xamlNamespace = XamlNamespace
                .Map("root")
                .With(configuredAssemblyWithNamespaces);
            directory.AddNamespace(xamlNamespace);

            var sut = new XamlToTreeParser(directory, new Context.MetadataProvider());
            var tree = sut.Parse(File.ReadAllText("Sample.xml"));
            return tree;
        }
    }
}