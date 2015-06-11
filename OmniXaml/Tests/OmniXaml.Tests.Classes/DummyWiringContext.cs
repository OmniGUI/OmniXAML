namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;
    using Another;
    using Builder;
    using Typing;

    public static class DummyWiringContext
    {
        public static WiringContext Create()
        {
            var rootType = typeof (DummyClass);
            var anotherType = typeof (Foreigner);

            var builder = new WiringContextBuilder();

            var definitionForRoot = XamlNamespace
                .Map("root")
                .With(
                    new[]
                    {
                        Route.Assembly(rootType.Assembly)
                            .WithNamespaces(new[] {rootType.Namespace})
                    });

            var definitionForAnother = XamlNamespace
                .Map("another")
                .With(
                    new[]
                    {
                        Route.Assembly(anotherType.Assembly)
                            .WithNamespaces(new[] {anotherType.Namespace})
                    });

            var contentProperties = ContentProperties.DefinedInAssemblies(new[] {rootType.Assembly});

            builder.WithNamespaces(new List<XamlNamespace> {definitionForRoot, definitionForAnother})
                .WithContentProperties(contentProperties)
                .WithNsPrefixes(
                    new List<PrefixRegistration>
                    {
                        new PrefixRegistration(string.Empty, "root"),
                        new PrefixRegistration("x", "another")
                    });

            return builder.Build();
        }
    }
}