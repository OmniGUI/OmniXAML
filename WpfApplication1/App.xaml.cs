using System.Collections.Generic;
using System.Windows;

namespace WpfApplication1
{
    using System.Windows.Controls;
    using Context;
    using OmniXaml;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var objectBuilder = new ObjectBuilder(new InstanceCreator(), Registrator.GetSourceValueConverter());
            var contructionNodes = new[]
            {
                new ContructionNode
                {
                    InstanceType = typeof(TextBlock),
                    Assignments = new[]
                    {
                        new PropertyAssignment
                        {
                            Property = new Property {Name = "Text"},
                            SourceValue = "Flipote",
                        },
                        new PropertyAssignment
                        {
                            Property = new Property {Name = "Background"},
                            SourceValue = "Red",
                        },
                    }
                },
                new ContructionNode
                {
                    InstanceType = typeof(TextBlock),
                    Assignments = new[]
                    {
                        new PropertyAssignment
                        {
                            Property = new Property {Name = "Text"},
                            SourceValue = "Flipotemos",
                        },
                        //new PropertyAssignment
                        //{
                        //    Property = new Property {Name = "Grid.Column"},
                        //    SourceValue = "1",
                        //},
                    }
                }
            };

            var window = (Window) objectBuilder.Create(
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
                        },
                        new PropertyAssignment
                        {
                            Property = new Property
                            {
                                Name = "FontSize",
                            },
                            SourceValue = "20",
                        },
                        new PropertyAssignment
                        {
                            Property = new Property
                            {
                                Name = "Padding",
                            },
                            SourceValue = "10, 20, 30, 40",
                        },
                        new PropertyAssignment
                        {
                            Property = new Property
                            {
                                Name = "Content",
                            },

                            Children = new[]
                            {
                                new ContructionNode
                                {
                                    InstanceType = typeof(Grid),
                                    Assignments = new[]
                                    {
                                        new PropertyAssignment
                                        {
                                            Property = new Property {Name = "Children"},
                                            Children = contructionNodes,
                                        },
                                        new PropertyAssignment
                                        {
                                            Property = new Property {Name = "ColumnDefinitions"},
                                            Children = new[]
                                            {
                                                new ContructionNode
                                                {
                                                    InstanceType = typeof(ColumnDefinition),
                                                    Assignments =
                                                        new[] {new PropertyAssignment {Property = new Property() {Name = "Width"}, SourceValue = "2*",},}
                                                },
                                                new ContructionNode
                                                {
                                                    InstanceType = typeof(ColumnDefinition),
                                                    Assignments =
                                                        new[] {new PropertyAssignment {Property = new Property() {Name = "Width"}, SourceValue = "1*",},}
                                                },
                                            },
                                        },
                                    },
                                },
                            }
                        }

                    }
                });

            //window.Content = new TextBlock { Text = "Hola" };
            window.Show();
            MainWindow = window;
        }
    }
}
