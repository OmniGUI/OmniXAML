namespace TestApplication
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Resources;
    using Xaml.Tests.Resources;

    public interface IXamlSnippetProvider
    {
        IList Snippets { get; }
    }

    public class XamlSnippetProvider : IXamlSnippetProvider
    {
        private readonly ICollection<Snippet> snippets = new Collection<Snippet>();

        public XamlSnippetProvider()
        {
            var assembly = typeof(Dummy).Assembly;
            var resourceIds = new[] {"Xaml.Tests.Resources.Dummy.resources", "Xaml.Tests.Resources.Wpf.resources" };

            foreach (var resourceId in resourceIds)
            {
                try
                {
                    using (var reader = new ResourceReader(assembly.GetManifestResourceStream(resourceId)))
                    {
                        var enumerator = reader.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            var name = enumerator.Key as string;
                            var xaml = enumerator.Value as string;
                            if (name != null && xaml != null)
                            {
                                snippets.Add(new Snippet(name, xaml));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Cannot load snippets. Exception: {e}");
                }
            }  
        }

        public IList Snippets => new ReadOnlyCollection<Snippet>(snippets.ToList());
    }

    public class Snippet
    {
        public Snippet(string name, string xaml)
        {
            Name = name;
            Xaml = xaml;
        }

        public string Name { get; set; }
        public string Xaml { get; set; }
    }
}