using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Logging.Serilog;
using Avalonia.Themes.Default;
using Avalonia.Markup.Xaml;
using Serilog;

namespace AvaloniaApp
{
    using System.IO;
    using Context;
    using OmniXaml;
    using OmniXaml.TypeLocation;

    class App : Application
    {

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        static void Main(string[] args)
        {
            InitializeLogging();
            AppBuilder.Configure<App>()
                .UseWin32()
                .UseDirect2D1()
                .SetupWithoutStarting();

            var objectBuilder = new ObjectBuilder(new InstanceCreator(), Registrator.GetSourceValueConverter());
            var cons = GetConstructionNode();

            var window = (Window)objectBuilder.Create(cons);
            window.Show();

            Current.Run(window);
        }

        private static ConstructionNode GetConstructionNode()
        {
            var type = typeof(Window);
            var ass = type.Assembly;

            ITypeDirectory directory = new TypeDirectory();

            directory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));

            var configuredAssemblyWithNamespaces = Route
                .Assembly(ass)
                .WithNamespaces("Avalonia.Controls");
            var xamlNamespace = XamlNamespace
                .Map("root")
                .With(configuredAssemblyWithNamespaces);
            directory.AddNamespace(xamlNamespace);

            var sut = new XamlToTreeParser(directory, new MetadataProvider());
            var tree = sut.Parse(File.ReadAllText("Sample.xml"));
            return tree;
        }


        public static void AttachDevTools(Window window)
        {
#if DEBUG
            DevTools.Attach(window);
#endif
        }

        private static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }
    }
}
