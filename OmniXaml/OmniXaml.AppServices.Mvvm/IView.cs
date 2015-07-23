namespace OmniXaml.AppServices.Mvvm
{
    public interface IView
    {
        void Show();
        void SetViewModel(object viewModel);
    }
}