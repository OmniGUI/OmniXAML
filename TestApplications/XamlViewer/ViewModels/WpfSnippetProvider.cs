namespace XamlViewer.ViewModels
{
    using System.Collections.Generic;

    public class WpfSnippetProvider : IXamlSnippetProvider
    {
        public IEnumerable<Snippet> Snippets { get; }
    }
}