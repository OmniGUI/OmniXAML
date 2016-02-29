namespace OmniXaml.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using Services;

    public class DummyAutoInflatingTypeFactory : AutoInflatingTypeFactory
    {
        public DummyAutoInflatingTypeFactory(ITypeFactory typeFactory, IInflatableTranslator inflatableTranslator, Func<ITypeFactory, ILoader> xamlLoaderFactory)
            : base(typeFactory, inflatableTranslator, xamlLoaderFactory)
        {
        }

        public override IEnumerable<Type> Inflatables { get; } = new Collection<Type> {typeof (Window), typeof (UserControl)};
    }
}