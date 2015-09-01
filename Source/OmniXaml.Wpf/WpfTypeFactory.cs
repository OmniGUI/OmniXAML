namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using Services;
    using Services.DotNetFx;

    public class WpfTypeFactory : AutoInflatingTypeFactory
    {
        readonly IEnumerable<Type> inflatables = new Collection<Type> { typeof(Window), typeof(UserControl) };

        public WpfTypeFactory() : this(new TypeFactory())
        {
        }

        public WpfTypeFactory(ITypeFactory typeFactory) : base(typeFactory, new InflatableTranslator(), t => new WpfCoreXamlLoader(t))
        {
        }

        public override IEnumerable<Type> Inflatables => inflatables;
    }
}