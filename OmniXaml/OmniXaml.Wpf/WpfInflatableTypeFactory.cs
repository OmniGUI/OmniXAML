namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using AppServices;
    using AppServices.NetCore;

    public class WpfInflatableTypeFactory : InflatableTypeFactory
    {
        public WpfInflatableTypeFactory() : base(new TypeFactory(), new NetCoreResourceProvider(), new NetCoreTypeToUriLocator(), _ => new WpfXamlStreamLoader())
        {            
            Inflatables = new Collection<Type> { typeof(Window), typeof(UserControl) };
        }     
    }
}