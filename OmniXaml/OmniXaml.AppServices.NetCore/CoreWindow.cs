namespace OmniXaml.AppServices.NetCore
{
    using System.Threading.Tasks;
    using System.Windows;
    using Mvvm;

    public class CoreWindow : Window, IView
    {
        public Task ShowDialog()
        {
            throw new System.NotImplementedException();
        }

        public void SetViewModel(object viewModel)
        {
            this.DataContext = viewModel;
        }
    }
}