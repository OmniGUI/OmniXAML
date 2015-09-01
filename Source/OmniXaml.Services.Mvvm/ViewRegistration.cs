namespace OmniXaml.Services.Mvvm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ViewRegistration
    {
        public string Token { get; }
        public Type ViewType { get; }

        public ViewRegistration(string token, Type viewType)
        {
            Token = token;
            ViewType = viewType;
        }

        public static IEnumerable<ViewRegistration> FromTypes(IEnumerable<Type> types)
        {
            return from assembly in types
                   let attribute = assembly.GetTypeInfo().GetCustomAttribute<ViewTokenAttribute>()
                   where attribute != null
                   select new ViewRegistration(attribute.Token, attribute.ViewType);
        }
    }
}