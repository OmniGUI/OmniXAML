namespace OmniXaml.Services.Mvvm
{
    using System.Collections.Generic;

    public interface IViewFactory
    {
        void RegisterView(ViewRegistration viewRegistration);
        void Show(string token);
        void RegisterViews(IEnumerable<ViewRegistration> viewRegistrations);
        void ShowDialog(string token);
        IView GetWindow(string token);
    }
}