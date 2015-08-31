namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using AppServices;
    using AppServices.NetCore;

    public class WpfTypeFactory : InflatableTypeFactory
    {
        readonly IEnumerable<Type> inflatables = new Collection<Type> { typeof(Window), typeof(UserControl) };

        public WpfTypeFactory() : base(new TypeFactory(), new InflatableTranslator(), typeFactory => new WpfXamlLoader(typeFactory))
        {
        }

        public WpfTypeFactory(ITypeFactory typeFactory) : base(typeFactory, new InflatableTranslator(), t => new WpfXamlLoader(t))
        {
        }

        public override IEnumerable<Type> Inflatables => inflatables;
    }
}