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

    public class DummyLoaderViewModel : XamlVisualizerViewModel
    {
        private ObservableCollection<InstanceNodeViewModel> representation;
        private InstanceNodeViewModel selectedItem;
        private Snippet selectedSnippet;
        private string xaml;

        public DummyLoaderViewModel()
        {
            IXamlSnippetProvider snippetsProvider = new XamlSnippetProvider(typeof(XamlResources).Assembly, "Xaml.Tests.Resources.Dummy.resources");
            Snippets = snippetsProvider.Snippets;
            Xaml = XamlResources.ChildCollection;
            LoadCommand = new RelayCommand(o => LoadXaml());
            
            SetSelectedItemCommand = new RelayCommand(o => SetSelectedItem((InstanceNodeViewModel)o));
            SetSelectedSnippetCommand = new RelayCommand(o => SetSelectedSnippet());
            WiringContext = DummyWiringContext.Create();
        }

        public IList Snippets { get; set; }

        public InstanceNodeViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<InstanceNodeViewModel> Representation
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
                window.DataContext = new TestViewModel();
                window.Show();

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
            Xaml = string.Copy(SelectedSnippet.Xaml);
        }

        private void SetSelectedItem(InstanceNodeViewModel o)
        {
            SelectedItem = o;
        }

        private void LoadXaml()
        {
            try
            {
                var loader = new XamlXmlLoader(new ObjectAssembler(WiringContext), WiringContext);

                var rootObject = loader.Load(Xaml);
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

        private static ObservableCollection<InstanceNodeViewModel> ConvertToViewNodes(object result)
        {
            return new ObservableCollection<InstanceNodeViewModel>
            {
                new InstanceNodeViewModel(result)
            };
        }
    }
}