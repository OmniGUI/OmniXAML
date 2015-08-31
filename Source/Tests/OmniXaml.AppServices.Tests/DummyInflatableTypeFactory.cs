namespace OmniXaml.AppServices.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using OmniXaml.Tests.Classes.WpfLikeModel;

    public class DummyInflatableTypeFactory : InflatableTypeFactory
    {
        public DummyInflatableTypeFactory(ITypeFactory typeFactory, IInflatableTranslator inflatableTranslator, Func<ITypeFactory, IXamlLoader> loaderFactory)
            : base(typeFactory, inflatableTranslator, loaderFactory)
        {
        }

        public override IEnumerable<Type> Inflatables { get; } = new Collection<Type> {typeof (Window), typeof (UserControl)};
    }
}