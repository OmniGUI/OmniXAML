namespace XamlViewer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public interface IXamlSnippetProvider
    {
        IEnumerable<Snippet> Snippets { get; }
    }

    class SnippetProvider : IXamlSnippetProvider
    {
        public SnippetProvider(string folder)
        {
            Snippets =
                Xaml.Tests.Resources.File.GetFiles(folder).Select(GetSnippet);
        }

        private static Snippet GetSnippet(string s)
        {
            return new Snippet(Path.GetFileNameWithoutExtension(s), Xaml.Tests.Resources.File.LoadAsString(s));
        }

        public IEnumerable<Snippet> Snippets { get; }
    }
}