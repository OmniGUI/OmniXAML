namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using Another;
    using Builder;
    using Typing;

    public static class DummyWiringContext
    {
        public static WiringContext Create()
        {
            var builder = new WiringContextBuilder();

            var rootType = typeof(DummyClass);
            var anotherType = typeof(Foreigner);

            var definitionForRoot = XamlNamespace
                .CreateMapFor(rootType.Namespace)
                .FromAssembly(rootType.Assembly)
                .To("root");

            var definitionForAnother = XamlNamespace
                .CreateMapFor(anotherType.Namespace)
                .FromAssembly(anotherType.Assembly)
                .To("another");

            var contentProperties = ContentProperties.DefinedInAssemblies(new[] { rootType.Assembly });

            builder
                .WithContentProperties(contentProperties)
                .WithNamespaces(new List<XamlNamespace> { definitionForRoot, definitionForAnother })
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