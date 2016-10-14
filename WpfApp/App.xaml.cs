namespace WpfApplication1
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
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
            //var contructionNodes = new[]
            //{
            //    new ConstructionNode
            //    {
            //        InstanceType = typeof(TextBlock),
            //        Assignments = new[]
            //        {
            //            new PropertyAssignment
            //            {
            //                Property = new Property(typeof(TextBlock), "Text") ,
            //                SourceValue = "Flipote",
            //            },
            //            new PropertyAssignment
            //            {
            //                Property = new Property(typeof(TextBlock), "Background"),
            //                SourceValue = "Red",
            //            },
            //        }
            //    },
            //    new ConstructionNode
            //    {
            //        InstanceType = typeof(TextBlock),
            //        Assignments = new[]
            //        {
            //            new PropertyAssignment
            //            {
            //                Property = new Property(typeof(TextBlock), "Text"),
            //                SourceValue = "Flipotemos",
            //            },
            //            //new PropertyAssignment
            //            //{
            //            //    Property = new Property {Name = "Grid.Column"},
            //            //    SourceValue = "1",
            //            //},
            //        }
            //    }
            //};

            //var window = (Window) objectBuilder.Create(
            //    new ConstructionNode
            //    {
            //        InstanceType = typeof(Window),
            //        Assignments = new List<PropertyAssignment>
            //        {
            //            new PropertyAssignment
            //            {
            //                Property = new Property
            //                {
            //                    Name = "Title",
            //                },
            //                SourceValue = "Pepito",
            //            },
            //            new PropertyAssignment
            //            {
            //                Property = new Property
            //                {
            //                    Name = "FontSize",
            //                },
            //                SourceValue = "20",
            //            },
            //            new PropertyAssignment
            //            {
            //                Property = new Property
            //                {
            //                    Name = "Padding",
            //                },
            //                SourceValue = "10, 20, 30, 40",
            //            },
            //            new PropertyAssignment
            //            {
            //                Property = new Property
            //                {
            //                    Name = "Content",
            //                },

            //                Children = new[]
            //                {
            //                    new ConstructionNode
            //                    {
            //                        InstanceType = typeof(Grid),
            //                        Assignments = new[]
            //                        {
            //                            new PropertyAssignment
            //                            {
            //                                Property = new Property {Name = "Children"},
            //                                Children = contructionNodes,
            //                            },
            //                            new PropertyAssignment
            //                            {
            //                                Property = new Property {Name = "ColumnDefinitions"},
            //                                Children = new[]
            //                                {
            //                                    new ConstructionNode
            //                                    {
            //                                        InstanceType = typeof(ColumnDefinition),
            //                                        Assignments =
            //                                            new[] {new PropertyAssignment {Property = new Property() {Name = "Width"}, SourceValue = "2*",},}
            //                                    },
            //                                    new ConstructionNode
            //                                    {
            //                                        InstanceType = typeof(ColumnDefinition),
            //                                        Assignments =
            //                                            new[] {new PropertyAssignment {Property = new Property() {Name = "Width"}, SourceValue = "1*",},}
            //                                    },
            //                                },
            //                            },
            //                        },
            //                    },
            //                }
            //            }

            //        }
            //    });

            var contructionNode = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<PropertyAssignment>
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty(typeof(Window), "Content"),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode(typeof(Grid))
                            {
                                Assignments = new List<PropertyAssignment>
                                {
                                    new PropertyAssignment
                                    {
                                        Property = Property.RegularProperty<Grid>(g => g.Children),
                                        Children = new List<ConstructionNode>
                                        {
                                            new ConstructionNode(typeof(TextBlock))
                                            {
                                                Assignments =
                                                    new List<PropertyAssignment>
                                                    {
                                                        new PropertyAssignment
                                                        {
                                                            Property = Property.RegularProperty(typeof(TextBlock), "Text"),
                                                            SourceValue = "Saludos cordiales!!"
                                                        }
                                                    }
                                            },
                                            new ConstructionNode(typeof(TextBlock))
                                            {
                                                Assignments = new List<PropertyAssignment>
                                                {
                                                    new PropertyAssignment {Property = Property.FromAttached<Grid>("Column"), SourceValue = "1"},
                                                    new PropertyAssignment
                                                    {
                                                        Property = Property.RegularProperty<TextBlock>(t => t.Text),
                                                        SourceValue = "Saludos cordiales!!"
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new PropertyAssignment
                                    {
                                        Property = Property.RegularProperty<Grid>(d => d.ColumnDefinitions),
                                        Children = new List<ConstructionNode>
                                        {
                                            new ConstructionNode(typeof(ColumnDefinition))
                                            {
                                                Assignments = new List<PropertyAssignment>
                                                {
                                                    new PropertyAssignment
                                                    {
                                                        Property = Property.RegularProperty<ColumnDefinition>(d => d.Width),
                                                        SourceValue = "3*"
                                                    }
                                                }
                                            },
                                            new ConstructionNode(typeof(ColumnDefinition))
                                            {
                                                Assignments = new List<PropertyAssignment>
                                                {
                                                    new PropertyAssignment
                                                    {
                                                        Property = Property.RegularProperty<ColumnDefinition>(d => d.Width),
                                                        SourceValue = "2*"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty(typeof(Window), "Title"),
                        SourceValue = "¿Cómo va la cosa?"
                    },
                    new PropertyAssignment
                    {
                        Property = Property.FromAttached(typeof(Grid), "Row"),
                        SourceValue = "1"
                    }
                }
            };

            var cons = GetConstructionNode();

            var window = (Window) objectBuilder.Create(cons);

            //window.Content = new TextBlock { Text = "Hola" };
            window.Show();
            MainWindow = window;
        }

        private ConstructionNode GetConstructionNode()
        {
            var type = typeof(Window);
            var ass = type.Assembly;

            var sut = new XamlToTreeParser(ass, new[] {type.Namespace, typeof(TextBlock).Namespace}, new ContentPropertyProvider());
            var tree = sut.Parse(File.ReadAllText("MainWindow.xml"));
            return tree;
        }
    }
}