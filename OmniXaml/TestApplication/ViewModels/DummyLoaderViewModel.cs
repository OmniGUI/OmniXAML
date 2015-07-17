namespace TestApplication.ViewModels
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Assembler;
    using OmniXaml.NewAssembler;
    using OmniXaml.Tests.Classes;
    using XamlResources = Xaml.Tests.Resources.Dummy;

    public class DummyLoaderViewModel : XamlVisualizerViewModel
    {
        private ObservableCollection<InstanceNodeViewModel> representation;
        private InstanceNodeViewModel selectedItem;
        private Snippet selectedSnippet;

        public DummyLoaderViewModel()
        {
            IXamlSnippetProvider snippetsProvider = new XamlSnippetProvider(typeof(XamlResources).Assembly, "Xaml.Tests.Resources.Dummy.resources");
            Snippets = snippetsProvider.Snippets;
            Xaml = XamlResources.ChildCollection;
            SetSelectedItemCommand = new RelayCommand(o => SelectedItem = (InstanceNodeViewModel)o, o => o != null);
            LoadCommand = new RelayCommand(Execute.Safely(o => LoadXaml()), o => Xaml != string.Empty);
            WiringContext = DummyWiringContext.Create(new TypeFactory());
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

        public ICommand LoadCommand { get; private set; }
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

        private void LoadXaml()
        {
            var loader = new XamlLoader(WiringContext, new DefaultObjectAssemblerFactory(WiringContext));

            var rootObject = XamlLoaderExtensions.Load(loader, Xaml);
            Representation = ConvertToViewNodes(rootObject);
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