namespace XamlViewer.ViewModels
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Wpf;
    using XamlViewer;
    using Xaml.Tests.Resources;

    public class WpfLoaderViewModel : XamlVisualizerViewModel
    {
        private Snippet selectedSnippet;

        public WpfLoaderViewModel()
        {
            IXamlSnippetProvider snippetsProvider = new XamlSnippetProvider(typeof(Dummy).Assembly, "Xaml.Tests.Resources.Wpf.resources");
            Snippets = snippetsProvider.Snippets;
            LoadCommand = new RelayCommand(o => LoadXamlForWpf(), o => Xaml != string.Empty);
            WiringContext = new WpfWiringContext(new TypeFactory());
        }


        public IList Snippets { get; set; }

        public ICommand LoadCommand { get; private set; }

        public Snippet SelectedSnippet
        {
            get { return selectedSnippet; }
            set
            {
                selectedSnippet = value;
                Xaml = string.Copy(SelectedSnippet.Xaml);
                OnPropertyChanged();
            }
        }

        private void LoadXamlForWpf()
        {
            try
            {
                var visualTree = new WpfXamlLoader().Load(Xaml);

                var window = GetVisualizerWindow(visualTree);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.DataContext = new TestViewModel();
                window.Show();

            }
            catch (XamlLoadException e)
            {
                ShowProblemLoadingError(e);
            }
        }

        private static Window GetVisualizerWindow(object visualTree)
        {
            var tree = visualTree as Window;
            if (tree != null)
            {
                return tree;
            }

            var window = new Window { Content = visualTree };
            return window;
        }

        private static void ShowProblemLoadingError(XamlLoadException e)
        {
            MessageBox.Show(
                $"There has been a problem loading the XAML at line: {e.LineNumber} pos: {e.LinePosition}. Detailed exception: \n\nException:\n{e.ToString().GetFirstNChars(500)}",
                "Load problem");
        }
    }

}