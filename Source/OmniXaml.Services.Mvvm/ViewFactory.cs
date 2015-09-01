namespace OmniXaml.Services.Mvvm
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ViewFactory : IViewFactory
    {
        private readonly ITypeFactory typeFactory;

        public ViewFactory(ITypeFactory typeFactory)
        {
            this.typeFactory = typeFactory;
        }

        private readonly Dictionary<string, Type> viewRegistrations = new Dictionary<string, Type>();

        public void RegisterView(ViewRegistration viewRegistration)
        {
            viewRegistrations.Add(viewRegistration.Token, viewRegistration.ViewType);
        }

        public void Show(string token)
        {
            var view = CreateUsingToken(token);
            TryInjectViewModel(view);
            view.Show();
        }

        private void TryInjectViewModel(IView view)
        {
            var viewType = view.GetType();
            var viewModelType = GetViewModelType(viewType);

            if (viewModelType != null)
            {
                var viewModel = typeFactory.Create(viewModelType);
                view.SetViewModel(viewModel);
            }
        }

        private static Type GetViewModelType(Type viewType)
        {
            var name = viewType.Namespace + "." + viewType.Name + "ViewModel";
            var viewModelType = viewType.GetTypeInfo().Assembly.GetType(name);
            return viewModelType;
        }

        private IView CreateUsingToken(string token)
        {
            var type = viewRegistrations[token];
            var view = (IView) typeFactory.Create(type);
            return view;
        }

        public void RegisterViews(IEnumerable<ViewRegistration> viewRegistrations)
        {
            foreach (var viewRegistration in viewRegistrations)
            {
                RegisterView(viewRegistration);
            }
        }

        public void ShowDialog(string token)
        {
            var view = CreateUsingToken(token);
            TryInjectViewModel(view);
            view.ShowDialog();
        }

        public IView GetWindow(string token)
        {
            var view = CreateUsingToken(token);
            TryInjectViewModel(view);
            return view;
        }
    }
}