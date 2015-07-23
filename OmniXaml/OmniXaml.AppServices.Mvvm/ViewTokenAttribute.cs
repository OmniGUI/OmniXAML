namespace OmniXaml.AppServices.Mvvm
{
    using System;

    public class ViewTokenAttribute : Attribute
    {
        public string Token { get; }
        public Type ViewType { get; }

        public ViewTokenAttribute(string token, Type viewType)
        {
            this.Token = token;
            this.ViewType = viewType;
        }
    }
}
