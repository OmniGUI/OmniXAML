namespace TestApplication
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Assembler;
    using OmniXaml.Tests.Classes;
    using OmniXaml.Wpf;
    using XamlResources = Xaml.Tests.Resources.Dummy;

    public class MainViewModel : ViewModel
    {
        private ObservableCollection<Node> representation;
        private Node selectedItem;
        private Snippet selectedSnippet;
        private string xaml;

        public MainViewModel()
        {
            IXamlSnippetProvider snippetsProvider = new XamlSnippetProvider();
            Snippets = snippetsProvider.Snippets;
            Xaml = XamlResources.ChildCollection;
            LoadCommand = new RelayCommand(o => LoadXaml());
            LoadForWpfCommand = new RelayCommand(o => LoadXamlForWpf());
            SetSelectedItemCommand = new RelayCommand(o => SetSelectedItem((Node)o));
            SetSelectedSnippetCommand = new RelayCommand(o => SetSelectedSnippet());
        }

        private WiringContext ContextForWpf => WpfContextFactory.Create();

        private WiringContext ContextForTestClasses => DummyWiringContext.Create();

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

        public ObservableCollection<Node> Representation
        {
            get { return representation; }
            set
            {
                representation = value;
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

        public ICommand LoadCommand { get; private set; }
        public ICommand LoadForWpfCommand { get; private set; }
        public ICommand SetSelectedItemCommand { get; private set; }
        public ICommand SetSelectedSnippetCommand { get; private set; }

        public Snippet SelectedSnippet
        {
            get { return selectedSnippet; }
            set
            {
                selectedSnippet = value;
                OnPropertyChanged();
            }
        }

        private void LoadXamlForWpf()
        {
            try
            {
                var localLoader = new XamlXmlLoader(new ObjectAssembler(ContextForWpf), ContextForWpf);

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Xaml)))
                {
                    var window = (Window)localLoader.Load(stream);
                    window.Show();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    $"There has been a problem loading the XAML.\n\nException:\n{GetFirstNChars(e.ToString(), 500)}",
                    "Load problem");
            }
        }

        private void SetSelectedSnippet()
        {
            Xaml = SelectedSnippet.Xaml;
        }

        private void SetSelectedItem(Node o)
        {
            SelectedItem = o;
        }

        private void LoadXaml()
        {
            try
            {
                var loader = new XamlXmlLoader(new ObjectAssembler(ContextForTestClasses), ContextForTestClasses);

                var rootObject = LoadFromString(Xaml, loader);
                Representation = ConvertToViewNodes(rootObject);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    $"There has been a problem loading the XAML.\n\nException:\n{GetFirstNChars(e.ToString(), 500)}",
                    "Load problem");
            }
        }

        private static string GetFirstNChars(string str, int max)
        {
            if (str.Length <= max)
            {
                return str;
            }
            return str.Substring(0, max) + "…";
        }

        private static object LoadFromString(string xaml, IXamlLoader loader)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml)))
            {
                return loader.Load(stream);
            }
        }

        private static ObservableCollection<Node> ConvertToViewNodes(object result)
        {
            return new ObservableCollection<Node>
            {
                new Node(result)
            };
        }
    }
}