namespace TestApplication.ViewModels
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Wpf;
    using Xaml.Tests.Resources;

    public class WpfLoaderViewModel : XamlVisualizerViewModel
    {
        private Snippet selectedSnippet;
        
        public WpfLoaderViewModel()
        {
            IXamlSnippetProvider snippetsProvider = new XamlSnippetProvider(typeof(Dummy).Assembly, "Xaml.Tests.Resources.Wpf.resources");
            Snippets = snippetsProvider.Snippets;
            LoadCommand = new RelayCommand(o => LoadXamlForWpf(), o => Xaml != string.Empty);
            WiringContext = WpfWiringContextFactory.Create();
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
                var localLoader = new WpfXamlLoader();

                var window = (Window)localLoader.Load(Xaml);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.DataContext = new TestViewModel();
                window.Show();

            }
            catch (Exception e)
            {
                ShowProblemLoadingError(e);
            }
        }

        private static void ShowProblemLoadingError(Exception e)
        {
            MessageBox.Show(
                $"There has been a problem loading the XAML.\n\nException:\n{e.ToString().GetFirstNChars(500)}",
                "Load problem");
        }
    }
}