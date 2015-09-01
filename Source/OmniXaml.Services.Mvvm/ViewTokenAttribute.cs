namespace OmniXaml.Services.Mvvm
{
    using System;

    public class ViewTokenAttribute : Attribute
    {
        public string Token { get; }
        public Type ViewType { get; }

        public ViewTokenAttribute(string token, Type viewType)
        {
            Token = token;
            ViewType = viewType;
        }
    }
}
