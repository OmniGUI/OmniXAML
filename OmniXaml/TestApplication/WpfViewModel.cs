namespace TestApplication
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Assembler;
    using OmniXaml.Wpf;
    using Xaml.Tests.Resources;

    public class WpfViewModel : ViewModel
    {
        private ObservableCollection<Node> representation;
        private Node selectedItem;
        private Snippet selectedSnippet;
        private string xaml;

        public WpfViewModel()
        {
            IXamlSnippetProvider snippetsProvider = new XamlSnippetProvider(typeof(Dummy).Assembly, "Xaml.Tests.Resources.Wpf.resources");
            Snippets = snippetsProvider.Snippets;
            LoadXamlCommand = new RelayCommand(o => LoadXamlForWpf(), o => IsValidXaml);
            SetSelectedItemCommand = new RelayCommand(o => SetSelectedItem((Node)o));
        }


        public IList Snippets { get; set; }

        public Node SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        public string Xaml
        {
            get { return xaml; }
            set
            {
                xaml = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadXamlCommand { get; private set; }
        public ICommand SetSelectedItemCommand { get; private set; }

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

        private void SetSelectedItem(Node o)
        {
            SelectedItem = o;
        }

        private bool IsValidXaml => Xaml != null;

        private static void ShowInvalidXamlError()
        {
            MessageBox.Show(
                $"Please, put some XAML into the text box",
                "Load problem");
        }

        private static void ShowProblemLoadingError(Exception e)
        {
            MessageBox.Show(
                $"There has been a problem loading the XAML.\n\nException:\n{GetFirstNChars(e.ToString(), 500)}",
                "Load problem");
        }

        private static string GetFirstNChars(string str, int max)
        {
            if (str.Length <= max)
            {
                return str;
            }
            return str.Substring(0, max) + "…";
        }
    }
}