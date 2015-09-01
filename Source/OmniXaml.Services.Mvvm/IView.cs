namespace OmniXaml.Services.Mvvm
{
    using System.Threading.Tasks;

    public interface IView
    {
        void Show();
        Task ShowDialog();
        void SetViewModel(object viewModel);
    }
}