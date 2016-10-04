using System.Collections.Generic;
using System.Windows;

namespace WpfApplication1
{
    using OmniXaml;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var objectBuilder = new ObjectBuilder(new InstanceCreator());
            var window = (Window)objectBuilder.Create(
                new ContructionNode
                {
                    InstanceType = typeof(Window),
                    Assignments = new List<PropertyAssignment>
                    {
                        new PropertyAssignment
                        {
                            Property = new Property
                            {
                                Name = "Title",                                                               
                            },
                            SourceValue = "Pepito",                     
                        }
                    }
                });

            window.Show();
            MainWindow = window;
        }
    }
}
