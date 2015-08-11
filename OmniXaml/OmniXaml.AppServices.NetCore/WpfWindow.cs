namespace OmniXaml.AppServices.NetCore
{
    using System.Threading.Tasks;
    using System.Windows;
    using Mvvm;

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