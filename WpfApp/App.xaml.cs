namespace WpfApplication1
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Effects;
    using Context;
    using OmniXaml;

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

            var sut = new XamlToTreeParser(ass, new[] {type.Namespace, typeof(TextBlock).Namespace, typeof(BlurEffect).Namespace }, new ContentPropertyProvider());
            var tree = sut.Parse(File.ReadAllText("Sample.xml"));
            return tree;
        }
    }
}