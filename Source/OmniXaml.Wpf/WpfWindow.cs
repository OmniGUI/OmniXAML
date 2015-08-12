namespace OmniXaml.Wpf
{
    using System.Threading.Tasks;
    using System.Windows;
    using AppServices.Mvvm;

    public class WpfWindow : Window, IView
    {
        public Task ShowDialog()
        {
            return new Task(() => this.ShowDialog());
        }        

        public void SetViewModel(object viewModel)
        {
            this.DataContext = viewModel;
        }
    }
}